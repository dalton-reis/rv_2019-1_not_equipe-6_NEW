
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

    [Header("Text Holders")]
    public TMP_TextHolder TimeLeft;
    public TMP_TextHolder LifeLeft;
    public TMP_TextHolder Question;
    public List<TMP_TextHolder> Answers;

    [Header("Events")]
    public UnityEvent lifeReachedZero = new UnityEvent();
    public UnityEvent noMoreQuestions = new UnityEvent();
    public QuestionDataEvent newQuestion = new QuestionDataEvent();
    public IntEvent lifeChanged = new IntEvent();
    public FloatEvent timeChanged = new FloatEvent();

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

    private void Start()
    {
        Debug.Assert(TimeLeft != null);
        Debug.Assert(LifeLeft != null);
        Debug.Assert(Question != null);
        Debug.Assert(Answers != null);

        // Setup initial callbacks
        newQuestion.AddListener(NewQuestionCallback);
        lifeChanged.AddListener(LifeChangedCallback);
        timeChanged.AddListener(TimeChangedCallback);
        lifeReachedZero.AddListener(LifeReachedZeroCallback);
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
        {
            if (CurrentLife > 0)
                CurrentLife--;
        }
    }

    private void NewQuestionCallback(QuestionData questionData)
    {
        if (questionData == null)
            return;

        CurrentTime = questionData.Time;

        TimeLeft.Text.text = $"{questionData.Time} {TimeSuffix}";
        Question.Text.text = questionData.Text;

        Debug.Assert(questionData.Answers.Count <= Answers.Count, $"Too many answers in this question, there are only {Answers.Count} places to put them");

        // Clear the answers before setting the new ones
        foreach (var answer in Answers)
            answer.Text.text = "";

        // Set the new answers
        for (int i = 0; i < questionData.Answers.Count; i++)
            Answers[i].Text.text = questionData.Answers[i].Text;
    }

    private void LifeChangedCallback(int life)
    {
        LifeLeft.Text.text = life.ToString();

        if (life <= 0) // Game Over
            lifeReachedZero.Invoke();
        else // Next Question
            NextQuestion();
    }

    private void TimeChangedCallback(float time)
    {
        TimeLeft.Text.text = $"{Math.Floor(time)} {TimeSuffix}";
    }

    private void LifeReachedZeroCallback()
    {
        Question.Text.text = "";

        foreach (var answer in Answers)
        {
            answer.Text.text = "";
        }
    }

    private void NoMoreQuestionsCallback()
    {

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
