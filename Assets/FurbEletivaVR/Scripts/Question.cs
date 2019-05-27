
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

public class Question : MonoBehaviour
{
    public TMP_Text QuestionText;
    public List<TMP_Text> AnswersText;

    public TMP_Text QuestionCountdownText;
    public TMP_Text AnswerCountdownText;

    public Color AnswerDisabledColor;
    public Color AnswerEnabledColor;

    [HideInInspector]
    public bool PauseCountdown = false;

    private QuestionData QuestionData;

    public UnityEvent Timeout;

    private void Awake()
    {
        foreach (var answerText in AnswersText)
        {
            answerText.raycastTarget = false;
            answerText.color = AnswerDisabledColor;
        }
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

        while (remainingQuestionSeconds > 0)
        {
            QuestionCountdownText.text = $"{remainingQuestionSeconds} s";
            remainingQuestionSeconds = Mathf.Max(0, remainingQuestionSeconds - 1);
            yield return new WaitForSeconds(remainingQuestionSeconds < 0 ? remainingQuestionSeconds : 1);

            while (PauseCountdown) // Pause
                yield return new WaitForSeconds(0.2f);
        }

        QuestionCountdownText.text = "";

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
