using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [Header("Solution")]
    public bool _isCorrect = false;
    public QuizManager _quizManager;
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

        _quizManager.Answer(_isCorrect);
    }
}