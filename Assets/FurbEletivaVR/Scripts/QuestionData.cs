
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "question", menuName = "FurbEletivaVR/New Question")]
public class QuestionData : ScriptableObject
{
    [TextArea()]
    public string Text;
    [Tooltip("Time to answer this question (in seconds)")]
    public float Time;

    public List<AnswerData> Answers;
}
