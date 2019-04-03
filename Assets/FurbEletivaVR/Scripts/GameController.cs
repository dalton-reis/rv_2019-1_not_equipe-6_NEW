
using Assets.FurbEletivaVR.Scripts.ExtensionMethods;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Question> QuestionDatabase;

    private List<Question> questionDatabaseClone;
    private Question currentQuestion;

    private void Start()
    {
        questionDatabaseClone = new List<Question>(QuestionDatabase);
        questionDatabaseClone.Shuffle();

        NextQuestion();
    }

    public void NextQuestion()
    {
        var questionsCount = questionDatabaseClone.Count;

        if (questionsCount == 0)
        {
            // There are no more questions
            // TODO: Call a event for this
            currentQuestion = null;
            return;
        }

        int lastElementIndex = questionsCount - 1;

        // Get the last question in the list
        currentQuestion = questionDatabaseClone[lastElementIndex];

        // And remove the last element (simulating a Queue)
        questionDatabaseClone.RemoveAt(lastElementIndex);
    }
}
