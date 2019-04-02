
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "question", menuName = "FurbEletivaVR/New Question")]
public class Question : ScriptableObject
{
    public string Text;

    public List<Answer> Answers;
}
