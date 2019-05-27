
using System;
using UnityEngine;

[Serializable]
public class AnswerData
{
    public string Text;
    [Tooltip("Answer audio to play")]
    public AudioClip Audio;
    public bool IsCorrect;
}