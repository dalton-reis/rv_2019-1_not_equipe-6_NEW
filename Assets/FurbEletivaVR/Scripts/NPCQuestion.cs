
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AIFollowPersonSimplePeople))]
public class NPCQuestion : MonoBehaviour
{
    public QuestionData question;

    public TMP_Text QuestionText;
    public List<TMP_Text> AnswersText;
    public GameObject MainCharacter;

    private AIFollowPersonSimplePeople aiFollowPerson;

    private void Awake()
    {
        aiFollowPerson = GetComponent<AIFollowPersonSimplePeople>();

        // Make sure to disabled the canvas and every answer at awake
        aiFollowPerson.QuestionCanvas.SetActive(false);
        foreach (var answerText in AnswersText)
            answerText.enabled = false;
    }

    private void Start()
    {
        QuestionText.text = question.Text;

        for (int i = 0; i < question.Answers.Count; i++)
        {
            var answerText = AnswersText[i];
            answerText.enabled = true;
            answerText.text = question.Answers[i].Text;
        }
    }

    public void AnswerQuestion(int index)
    {
        bool isCorrect = question.Answers[index].IsCorrect;

        var aiFollowPath = MainCharacter.GetComponent<AIFollowPathSimplePeople>();
        aiFollowPath.Stop = false;
        MainCharacter.GetComponent<LookAt>().m_Target = null;

        aiFollowPerson.QuestionCanvas.SetActive(false);
        if (isCorrect)
            aiFollowPerson.CharacterToFollow = MainCharacter.GetComponent<SimplePeopleCharacter>();
    }
}
