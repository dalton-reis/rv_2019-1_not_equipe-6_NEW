
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AIFollowPersonSimplePeople))]
public class NPCQuestion : MonoBehaviour
{
    public QuestionData question;
    public Question QuestionCanvas;

    public GameObject MainCharacter;

    private AIFollowPersonSimplePeople aiFollowPerson;

    public QuestionCanvasResult QuestionCanvasResult;
    public PointsCanvas PointsCanvas;

    public AudioMixer LightningMixer;
    public float NormalLightningVolume = 20.0f;
    public float ReadingLightningVolume = 0f;

    private Coroutine startQuestionCoroutine;

    private void Awake()
    {
        aiFollowPerson = GetComponent<AIFollowPersonSimplePeople>();

        // Make sure to disabled the canvas
        aiFollowPerson.QuestionCanvas.SetActive(false);
    }

    public void StartQuestion()
    {
        LightningMixer.SetFloat("Volume", ReadingLightningVolume);
        QuestionCanvas.SetupQuestionData(question);
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
        LightningMixer.SetFloat("Volume", NormalLightningVolume);

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
            StartCoroutine(PointsCanvas.Fail());

        if (startQuestionCoroutine != null)
            StopCoroutine(startQuestionCoroutine);
    }
}
