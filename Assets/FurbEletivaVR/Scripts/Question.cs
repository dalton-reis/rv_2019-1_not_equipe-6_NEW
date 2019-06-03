
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Question : MonoBehaviour
{
    public TMP_Text QuestionText;
    public List<TMP_Text> AnswersText;

    public TMP_Text QuestionCountdownText;
    public TMP_Text AnswerCountdownText;

    public Color AnswerDisabledColor;
    public Color AnswerHighlightedColor;
    public Color AnswerEnabledColor;

    public float SecondsDelayToStartReadingAnswers = 1f;
    public float SecondsDelayAfterReadingEachAnswer = 0.5f;

    [HideInInspector]
    public bool PauseCountdown = false;

    private QuestionData QuestionData;

    public UnityEvent Timeout;

    private AudioSource audioSource;

    private void Awake()
    {
        foreach (var answerText in AnswersText)
        {
            answerText.raycastTarget = false;
            answerText.color = AnswerDisabledColor;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void SetupQuestionData(QuestionData questionData)
    {
        QuestionData = questionData;

        QuestionText.text = QuestionData.Text;

        for (int i = 0; i < QuestionData.Answers.Count; i++)
        {
            var answerText = AnswersText[i];
            answerText.text = QuestionData.Answers[i].Text;
        }
    }

    public IEnumerator StartQuestion()
    {
        var remainingQuestionSeconds = QuestionData.Time;

        if (QuestionData.Audio != null)
        {
            // Adjust time
            float readingLength = QuestionData.Audio.length + SecondsDelayToStartReadingAnswers;
            if (readingLength > remainingQuestionSeconds)
                remainingQuestionSeconds = readingLength;

            audioSource.PlayOneShot(QuestionData.Audio);
        }

        remainingQuestionSeconds = Mathf.Ceil(remainingQuestionSeconds);

        while (remainingQuestionSeconds > 0)
        {
            QuestionCountdownText.text = $"{remainingQuestionSeconds} s";
            remainingQuestionSeconds = Mathf.Max(0, remainingQuestionSeconds - 1);
            yield return new WaitForSeconds(remainingQuestionSeconds < 0 ? remainingQuestionSeconds : 1);
        }

        QuestionCountdownText.text = "";

        // Read Answers
        for (int i = 0; i < QuestionData.Answers.Count; i++)
        {
            var answer = QuestionData.Answers[i];

            if (answer.Audio == null)
                continue;

            TMP_Text answerText = AnswersText[i];
            
            // Highlight the answer
            answerText.color = AnswerHighlightedColor;

            // Play the answer audio
            audioSource.PlayOneShot(answer.Audio);

            yield return new WaitForSeconds(answer.Audio.length + SecondsDelayAfterReadingEachAnswer);

            // De-Highlight the answer
            answerText.color = AnswerDisabledColor;
        }

        // Enable answers
        for (int i = 0; i < QuestionData.Answers.Count; i++)
        {
            TMP_Text answerText = AnswersText[i];

            answerText.raycastTarget = true;
            answerText.color = AnswerEnabledColor;
        }

        var remainingAnswerSeconds = QuestionData.AnswerTime;

        while (remainingAnswerSeconds > 0)
        {
            AnswerCountdownText.text = $"{remainingAnswerSeconds} s";
            remainingAnswerSeconds = Mathf.Max(0, remainingAnswerSeconds - 1);
            yield return new WaitForSeconds(remainingAnswerSeconds < 0 ? remainingAnswerSeconds : 1);

            while (PauseCountdown) // Pause
                yield return new WaitForSeconds(0.2f);
        }

        AnswerCountdownText.text = "";

        Timeout?.Invoke();
    }
}
