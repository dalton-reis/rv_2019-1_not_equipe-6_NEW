
using UnityEngine;

[RequireComponent(typeof(AIFollowPersonSimplePeople))]
public class NPCQuestion : MonoBehaviour
{
    public QuestionData question;
    public Question QuestionCanvas;

    public GameObject MainCharacter;

    private AIFollowPersonSimplePeople aiFollowPerson;

    public QuestionCanvasResult QuestionCanvasResult;
    public PointsCanvas PointsCanvas;

    private Coroutine startQuestionCoroutine;

    private void Awake()
    {
        aiFollowPerson = GetComponent<AIFollowPersonSimplePeople>();

        // Make sure to disabled the canvas
        aiFollowPerson.QuestionCanvas.SetActive(false);
    }

    private void Start()
    {
        QuestionCanvas.SetupQuestionData(question);
    }

    public void StartQuestion()
    {
        startQuestionCoroutine = StartCoroutine(QuestionCanvas.StartQuestion());
    }

    /// <summary>
    /// Pauses the question and answer countdown
    /// </summary>
    /// <param name="pause">Should pause?</param>
    public void PauseCountdown(bool pause)
    {
        QuestionCanvas.PauseCountdown = pause;
    }

    public void AnswerQuestion(int index)
    {
        bool isCorrect = question.Answers[index].IsCorrect;

        AnswerAssert(isCorrect);
    }

    public void AnswerAssert(bool isCorrect)
    {
        var aiFollowPath = MainCharacter.GetComponent<AIFollowPathSimplePeople>();
        aiFollowPath.Stop = false;
        MainCharacter.GetComponent<LookAt>().m_Target = null;

        QuestionCanvasResult.Answer(isCorrect);
        aiFollowPerson.QuestionCanvas.SetActive(false);
        if (isCorrect)
        {
            aiFollowPerson.CharacterToFollow = MainCharacter.GetComponent<SimplePeopleCharacter>();

            GameManager.Instance.Ok++;
            StartCoroutine(PointsCanvas.Ok());
        }
        else
        {
            GameManager.Instance.Fail++;
            StartCoroutine(PointsCanvas.Fail());
        }

        if (startQuestionCoroutine != null)
            StopCoroutine(startQuestionCoroutine);
    }
}
