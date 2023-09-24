using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuizManagerModels : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject _retryButton;
    [SerializeField] GameObject _questionPanel;
    [SerializeField] TMP_Text _questionText;
    [SerializeField] TMP_Text _choicesText;
    [SerializeField] TMP_Text _numberOfQuestions;

    [Header("Questions")]
    [SerializeField] List<QuizFormat> _questionsList;

    int _totalQuestions = 0;
    int _answeredQuestions = 1;
    int _correctAnswers = 0;

    public int _currentQuestionIndex = 0;

    [Header("Available Buttons")]
    [SerializeField] List<Button> _buttonsList;
    GameObject _buttonLeftObject;
    GameObject _buttonMiddleObject;
    GameObject _buttonRightObject;

    [Header("Scene Game Objects")]
    [SerializeField] GameObject _stageGuideCanvas;
    [SerializeField] GameObject _imageTarget;

    void Awake() 
    {
        _buttonLeftObject = _buttonsList[0].transform.GetChild(_currentQuestionIndex).gameObject;
        _buttonMiddleObject = _buttonsList[1].transform.GetChild(_currentQuestionIndex).gameObject;
        _buttonRightObject = _buttonsList[2].transform.GetChild(_currentQuestionIndex).gameObject;
    }

    void Start() => ShowQuiz();

    public void ShowQuiz() 
    {
        _totalQuestions = _questionsList.Count;
        GenerateQuestion();
    }

    void GenerateQuestion() 
    {
        _buttonLeftObject.SetActive(false);
        _buttonMiddleObject.SetActive(false);
        _buttonRightObject.SetActive(false);

        if (_currentQuestionIndex < _questionsList.Count)
        {
            _numberOfQuestions.text = _answeredQuestions + " / " + _totalQuestions;
            _questionText.text = _questionsList[_currentQuestionIndex].question;
            _choicesText.text = string.Empty;
            ResetButtons(true);
            SetAnswers(); // get next answer
            ActivateObject();
        }
        else
        {
            TerminateQuiz();
        }
    }

    void ResetButtons(bool active)
    {
        for (int i = 0; i < _buttonsList.Count; i++)
        {
            _buttonsList[i].interactable = active;
        }
    }

    void ActivateObject() 
    {
        _buttonLeftObject = _buttonsList[0].transform.GetChild(_currentQuestionIndex).gameObject;
        _buttonMiddleObject = _buttonsList[1].transform.GetChild(_currentQuestionIndex).gameObject;
        _buttonRightObject = _buttonsList[2].transform.GetChild(_currentQuestionIndex).gameObject;

        _buttonLeftObject.SetActive(true);
        _buttonMiddleObject.SetActive(true);
        _buttonRightObject.SetActive(true);
    }

    void SetAnswers()
    {
        for (int i = 0; i < _buttonsList.Count; i++)
        {
            _buttonsList[i].GetComponent<Image>().color = _buttonsList[i].GetComponent<ButtonManagerModels>()._startColor;
            _buttonsList[i].GetComponent<ButtonManagerModels>()._isCorrect = false; // to avoid having all buttons to be correct
            _choicesText.text += _questionsList[_currentQuestionIndex].answers[i] + "\n";

            if (_questionsList[_currentQuestionIndex].correctAnswer == i + 1)
            {
                _buttonsList[i].GetComponent<ButtonManagerModels>()._isCorrect = true;
            }
        }
    }

    public void Answer(bool PlayerAnswer)
    {
        if (PlayerAnswer == true)
            _correctAnswers++;

        RevealCorrectAnswer();
        _answeredQuestions++; // for every correct answer increase score by 1
        _currentQuestionIndex++;
        ResetButtons(false);
        StartCoroutine(WaitForNext()); // Get next question
    }

    void RevealCorrectAnswer()
    {
        for (int i = 0; i < _buttonsList.Count; i++)
        {
            if (_questionsList[_currentQuestionIndex].correctAnswer == i + 1)
            {
                _buttonsList[i].GetComponent<Image>().color = Color.green;
            }
            else
                _buttonsList[i].GetComponent<Image>().color = Color.red;
        }
    }

    void TerminateQuiz()
    {
        StopAllCoroutines();
        _answeredQuestions = 0;
        _imageTarget.SetActive(false);
        InitiateDialogueCanvas();
    }

    void InitiateDialogueCanvas()
    {
        _stageGuideCanvas.SetActive(true);
        _retryButton.SetActive(true);
        StageGuideDialogue stageDialogue = GameObject.Find("Dialogue Group").GetComponent<StageGuideDialogue>();
        stageDialogue._isQuizFinished = true;
        stageDialogue._stageGuide.text = "Tέλος! Απάντησες σωστά σε " + _correctAnswers + " από τις " + _totalQuestions + " ερωτήσεις.";
    }

    IEnumerator WaitForNext()
    {
        yield return new WaitForSecondsRealtime(1); // wait 1 second before executing generateQuestion() method
        GenerateQuestion();
    }

    IEnumerator LoadNextScene(float time, string stage)
    {
        yield return new WaitForSecondsRealtime(time);
        SceneManager.LoadScene(stage);
    }
}
