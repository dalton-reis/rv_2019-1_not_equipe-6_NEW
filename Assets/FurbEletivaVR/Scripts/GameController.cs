
using Assets.FurbEletivaVR.Scripts.ExtensionMethods;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class QuestionDataEvent : UnityEvent<QuestionData>
{
}

[Serializable]
public class IntEvent : UnityEvent<int>
{
}

[Serializable]
public class FloatEvent : UnityEvent<float>
{
}

public class GameController : MonoBehaviour
{
    [Header("Configuration")]
    public List<QuestionData> QuestionDatabase;
    [Min(0)]
    public int StartingLife = 4;

    public string TimeSuffix = "segundos";

    public WalkablePlane WalkablePlane;

    [Header("Text Holders")]
    public TMP_TextHolder TimeLeft;
    public TMP_TextHolder AnswerTimeLeft;
    public TMP_TextHolder LifeLeft;
    public TMP_TextHolder Question;
    public List<TMP_TextHolder> Answers;

    [Header("Events")]
    public UnityEvent lifeReachedZero = new UnityEvent();
    public UnityEvent noMoreQuestions = new UnityEvent();
    public QuestionDataEvent newQuestion = new QuestionDataEvent();
    public IntEvent lifeChanged = new IntEvent();
    public FloatEvent timeChanged = new FloatEvent();
    public UnityEvent timeReachedZero = new UnityEvent();

    private int currentLife;
    public int CurrentLife
    {
        get { return currentLife; }
        private set
        {
            if (currentLife == value)
                return;

            currentLife = value;
            lifeChanged.Invoke(currentLife);
        }
    }

    private float currentTime;
    public float CurrentTime
    {
        get { return currentTime; }
        private set
        {
            if (currentTime == value)
                return;

            currentTime = value;
            timeChanged.Invoke(currentTime);
        }
    }

    private float timeToAnswer;

    private Queue<QuestionData> questionDatabaseQueue;
    private QuestionData currentQuestion;

    public QuestionData CurrentQuestion
    {
        get { return currentQuestion; }
        private set
        {
            if (currentQuestion == value)
                return;

            currentQuestion = value;
            newQuestion.Invoke(currentQuestion);
        }
    }

    private Color slotWithoutAnswerColor = new Color32(0, 0, 0, 0);
    private Color slotWithAnswerColor = new Color32(48, 48, 48, 100);

    private void Start()
    {
        Debug.Assert(TimeLeft != null);
        Debug.Assert(AnswerTimeLeft != null);
        Debug.Assert(LifeLeft != null);
        Debug.Assert(Question != null);
        Debug.Assert(Answers != null);
        Debug.Assert(WalkablePlane != null);

        // Setup initial callbacks
        newQuestion.AddListener(NewQuestionCallback);
        lifeChanged.AddListener(LifeChangedCallback);
        lifeReachedZero.AddListener(LifeReachedZeroCallback);
        timeChanged.AddListener(TimeChangedCallback);
        timeReachedZero.AddListener(TimeReachedZeroCallback);
        noMoreQuestions.AddListener(NoMoreQuestionsCallback);

        currentLife = StartingLife;
        LifeLeft.Text.text = currentLife.ToString();

        QuestionDatabase.Shuffle(); // Shuffle the questions
        questionDatabaseQueue = new Queue<QuestionData>(QuestionDatabase); // And create the current queue

        NextQuestion();
    }

    private void Update()
    {
        CurrentTime = Math.Max(0, CurrentTime - Time.deltaTime);

        if (CurrentTime == 0)
            timeReachedZero.Invoke();
    }

    private void NewQuestionCallback(QuestionData questionData)
    {
        if (questionData == null)
            return;

        CurrentTime = questionData.Time;
        timeToAnswer = questionData.AnswerTime;

        TimeLeft.Text.text = $"{questionData.Time} {TimeSuffix}";
        Question.Text.text = questionData.Text;

        Debug.Assert(questionData.Answers.Count <= Answers.Count, $"Too many answers in this question, there are only {Answers.Count} places to put them");

        // Hide the answer time
        AnswerTimeLeft.gameObject.SetActive(false);

        // Clear the answers before setting the new ones
        foreach (var answer in Answers)
        { 
            answer.Text.text = "";
            answer.PanelImage.color = slotWithoutAnswerColor;
            answer.gameObject.SetActive(false);
        }

        // Set the new answers
        for (int i = 0; i < questionData.Answers.Count; i++)
        {
            var answer = Answers[i];

            answer.Text.text = questionData.Answers[i].Text;
            answer.PanelImage.color = slotWithAnswerColor;
        }

        WalkablePlane.SetFloorSize(questionData.Time * 3);
    }

    private void LifeChangedCallback(int life)
    {
        LifeLeft.Text.text = life.ToString();

        if (life <= 0) // Game Over
            lifeReachedZero.Invoke();
        else // Next Question
            NextQuestion();
    }

    private void LifeReachedZeroCallback()
    {
        Question.Text.text = "";

        foreach (var answer in Answers)
        {
            answer.Text.text = "";
        }
    }

    private void TimeChangedCallback(float time)
    {
        TimeLeft.Text.text = $"{Math.Ceiling(time)} {TimeSuffix}";
    }

    private void TimeReachedZeroCallback()
    {
        // Active all non-blank answers when `timeToAnswer` starts ticking
        if (timeToAnswer == CurrentQuestion.AnswerTime)
        {
            // Show the answer time
            AnswerTimeLeft.gameObject.SetActive(true);

            foreach (var answer in Answers)
                if (answer.Text.text != "")
                    answer.gameObject.SetActive(true);
        }

        timeToAnswer = Math.Max(0, timeToAnswer - Time.deltaTime);

        AnswerTimeLeft.Text.text = $"{Math.Ceiling(timeToAnswer)} {TimeSuffix}";

        // Decrease the life if the time to answer reached zero
        if (timeToAnswer == 0)
            if (CurrentLife > 0)
                CurrentLife--;
    }

    private void NoMoreQuestionsCallback()
    {
        WalkablePlane.SetFloorSize(1);
    }

    public void NextQuestion()
    {
        var questionsCount = questionDatabaseQueue.Count;

        if (questionsCount == 0)
        {
            // There are no more questions
            currentQuestion = null;

            noMoreQuestions.Invoke();
            return;
        }

        // Get the next question in the queue
        CurrentQuestion = questionDatabaseQueue.Dequeue();
    }
}
