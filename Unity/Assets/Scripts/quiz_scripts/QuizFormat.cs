using UnityEngine;

[System.Serializable]
public class QuizFormat
{
    [TextArea(5, 10)]
    public string question;
    public string[] answers;
    public int correctAnswer;
}
