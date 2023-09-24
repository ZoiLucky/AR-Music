using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class QuizManagerModels_backup : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject _retryButton;
    [SerializeField] GameObject _questionPanel;
    [SerializeField] TMP_Text _questionText;
    [SerializeField] TMP_Text _numberOfQuestions;

    int _totalQuestions = 0;
    int _answeredQuestions = 1;
    int _correctAnswers = 0;

    string _objectName;

    [Header("Questions")]
    [SerializeField] List<string> _questionsList;

    public int _currentQuestionIndex;

    [Header("Available Buttons")]
    [SerializeField] List<GameObject> _buttonList;
    private GameObject _buttonLeftObject;
    private GameObject _buttonMiddleObject;
    private GameObject _buttonRightObject;


    [Header("Scene Game Objects")]
    [SerializeField] GameObject _stageGuideCanvas;
    [SerializeField] GameObject _imageTarget;

    void OnAwake() 
    {
        _buttonLeftObject = _buttonList[0].transform.GetChild(_currentQuestionIndex).gameObject;
        _buttonMiddleObject = _buttonList[1].transform.GetChild(_currentQuestionIndex).gameObject;
        _buttonRightObject = _buttonList[2].transform.GetChild(_currentQuestionIndex).gameObject;
    }

    void Update()
    {
        if ((Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _objectName = hit.transform.name;
                switch (_objectName)
                {
                    case "Vivaldi": case "electricguitar": case "piano":
                        Answer(true);
                        break;
                    case "drums": case "pontiaki lura":
                        Answer(false);
                        break;
                    default:
                        break;
                }
            }
        }

        // Below code is for testing purposes [WEBCAM]
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        _objectName = hit.transform.name;
                        switch (_objectName)
                        {
                            case "Vivaldi":
                            case "electricguitar":
                            case "piano":
                                Answer(true);
                                break;
                            case "drums":
                            case "pontiaki lura":
                                Answer(false);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    void Start() => ShowQuiz();

    public void ShowQuiz() 
    {
        _totalQuestions = _questionsList.Count;
        GenerateQuestion();
    }

    void GenerateQuestion() 
    {
        _numberOfQuestions.text = _answeredQuestions + " / " + _totalQuestions;

        if (_questionsList.Count > 0)
        {
            _questionText.text = _questionsList[_currentQuestionIndex];
            ActivateObject();
        }
        else
        {
            TerminateQuiz();
        }
    }

    void ActivateObject() 
    {
        _buttonLeftObject = _buttonList[0].transform.GetChild(_currentQuestionIndex).gameObject;
        _buttonMiddleObject = _buttonList[1].transform.GetChild(_currentQuestionIndex).gameObject;
        _buttonRightObject = _buttonList[2].transform.GetChild(_currentQuestionIndex).gameObject;

        _buttonLeftObject.SetActive(true);
        _buttonMiddleObject.SetActive(true);
        _buttonRightObject.SetActive(true);
        Debug.Log("Left: " + _buttonLeftObject.gameObject.name + "/nMiddle: " + _buttonMiddleObject.gameObject.name + "/nRight: " + _buttonRightObject.gameObject.name);
    }

    void Answer(bool correctAnswer) 
    {
        Debug.Log("Answer!");

        if (correctAnswer)
            _correctAnswers++;

        _answeredQuestions++;
        Destroy(_buttonLeftObject);
        Destroy(_buttonMiddleObject);
        Destroy(_buttonRightObject);
        _questionsList.RemoveAt(_currentQuestionIndex);
        StartCoroutine(WaitForNext());
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
        stageDialogue._stageGuide.text = "Τέλος! Απάντησες σωστά σε " + _correctAnswers + " από τις " + _totalQuestions + " ερωτήσεις";
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
