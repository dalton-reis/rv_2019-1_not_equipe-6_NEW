using Assets.FurbEletivaVR.Scripts.ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDatabase : Singleton<QuestionDatabase>
{
    public List<QuestionData> Questions;

    private Queue<QuestionData> questionQueue;

    private void Awake()
    {
        ReloadQuestions();
    }

    public void ReloadQuestions()
    {
        var questionsCopy = new List<QuestionData>(Questions);
        questionsCopy.Shuffle();
        questionQueue = new Queue<QuestionData>(questionsCopy);
    }

    public QuestionData GetQuestion()
    {
        return questionQueue.Dequeue();
    }
}
