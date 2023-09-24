using UnityEngine;
using UnityEngine.UI;

public class ButtonManagerModels : MonoBehaviour
{
    [Header("Solution")]
    public bool _isCorrect = false;
    public QuizManagerModels _musicalQuiz;
    [Header("Button Color")]
    public Color _startColor;

    void Awake()
    {
        //_startColor = GetComponent<Image>().color = Color.white;
    }

    public void Answer()
    {
        GetComponent<Image>().color = _isCorrect ? Color.green : Color.red;
        Debug.Log(_isCorrect ? "Correct answer!" : "Wrong answer!");

        _musicalQuiz.Answer(_isCorrect);
    }
}