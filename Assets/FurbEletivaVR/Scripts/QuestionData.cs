
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "question", menuName = "FurbEletivaVR/New Question")]
public class QuestionData : ScriptableObject
{
    [TextArea]
    public string Text;

    [Tooltip("Question audio to play")]
    public AudioClip Audio;

    [Tooltip("Time to read the question (in seconds)")]
    public float Time = 10;

    [Tooltip("Time to answer the question (in seconds)")]
    public float AnswerTime = 10;

    public List<AnswerData> Answers;
}
