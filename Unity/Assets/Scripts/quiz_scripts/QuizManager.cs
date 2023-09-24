using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("Other class elements")]
    [SerializeField] GameObject _retryButton;

    [Header ("Questions")]
    [SerializeField] List<QuizFormat> _questionsList;

    int _currentQuestionIndex;

    [Header ("Available Buttons")]
    [SerializeField] List<Button> _buttonsList;

    [Header ("UI Elements")]
    [SerializeField] TMP_Text _questionText;
    [SerializeField] TMP_Text _choicesText;
    [SerializeField] GameObject _questionPanel;
    [SerializeField] TMP_Text _numberOfQuestions;

    int _totalQuestions = 0;
    int _answeredQuestions = 1;
    int _correctAnswers = 0;

    [Header ("Scene Game Objects")]
    [SerializeField] GameObject _stageGuideCanvas;
    [SerializeField] GameObject _imageTarget;

    void Start() => ShowQuiz();

    public void ShowQuiz()
    {
        RandomizeButtonPosition(_buttonsList); // Randomize answer's position
        _totalQuestions = _questionsList.Count;

        GenerateQuestion(); // Retrieve random question
    }

    void GenerateQuestion()
    {
        if (_questionsList.Count > 0)
        {
            _numberOfQuestions.text = _answeredQuestions + " / " + _totalQuestions; //UI element
            _currentQuestionIndex = Random.Range(0, _questionsList.Count); // get a random number from zero to number of our questions
            _questionText.text = _questionsList[_currentQuestionIndex].question;
            _choicesText.text = string.Empty;
            ResetButtons(true);
            RandomizeButtonPosition(_buttonsList);
            SetAnswers(); // get next answer
        }
        else
        {
            TerminateQuiz();
        }
    }

    private void RandomizeButtonPosition(List<Button> buttons)
    {
        foreach (Button button in buttons)
            button.transform.SetSiblingIndex(Random.Range(0, buttons.Count));
    }

    void ResetButtons(bool active)
    {
        for (int i = 0; i < _buttonsList.Count; i++)
        {
            _buttonsList[i].interactable = active;
            //_buttonsList[i].transform.GetChild(0).GetComponent<Text>().text = string.Empty;
        }
    }

    void DisableButtons(bool active)
    {
        for (int i = 0; i < _buttonsList.Count; i++)
        {
            _buttonsList[i].interactable = false;
            //_buttonsList[i].transform.GetChild(0).GetComponent<Text>().text = string.Empty;
        }
    }

    void SetAnswers()
    {
        for (int i = 0; i < _buttonsList.Count; i++)
        {
            _buttonsList[i].GetComponent<Image>().color = _buttonsList[i].GetComponent<ButtonManager>()._startColor;
            _buttonsList[i].GetComponent<ButtonManager>()._isCorrect = false; // to avoid having all buttons to be correct
            //_buttonsList[i].transform.GetChild(0).GetComponent<Text>().text = _questionsList[_currentQuestionIndex].answers[i];
            _choicesText.text += _questionsList[_currentQuestionIndex].answers[i] + "\n";

            if (_questionsList[_currentQuestionIndex].correctAnswer == i + 1)
            {
                _buttonsList[i].GetComponent<ButtonManager>()._isCorrect = true;
            }

            Debug.Log("Answer[" + i + "]: "  + _questionsList[_currentQuestionIndex].answers[i] + "\nCorrect Answer[" + _questionsList[_currentQuestionIndex].correctAnswer + "]: " + _questionsList[_currentQuestionIndex].answers[_questionsList[_currentQuestionIndex].correctAnswer-1]);
        }
    }

    public void Answer(bool PlayerAnswer)
    {
        if (PlayerAnswer == true)
            _correctAnswers++;

        RevealCorrectAnswer();
        _answeredQuestions++; // for every correct answer increase score by 1
        _questionsList.RemoveAt(_currentQuestionIndex); // remove answered question
        ResetButtons(false);
        StartCoroutine(WaitForNext()); // Get next question
    }

    void RevealCorrectAnswer() 
    {
        //_choicesText.text = "ΣΩΣΤΗ ΑΠΑΝΤΗΣΗ: " + _questionsList[_currentQuestionIndex].answers[_questionsList[_currentQuestionIndex].correctAnswer - 1];

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
