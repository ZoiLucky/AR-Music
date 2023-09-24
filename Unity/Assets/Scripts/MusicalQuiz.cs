using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MusicalQuiz : MonoBehaviour
{
    [Header ("Audio Objects")]
    [SerializeField] AudioClip[] _audioClips;
    [SerializeField] AudioSource _audioSource;

    [Header("UI Elements")]
    [SerializeField] ParticleSystem _musicNoteParticles;
    [SerializeField] GameObject _retryButton;
    [SerializeField] GameObject _questionPanel;
    [SerializeField] TMP_Text _questionText;
    [SerializeField] TMP_Text _choicesText;
    [SerializeField] TMP_Text _numberOfQuestions;

    int _totalQuestions = 0;
    int _answeredQuestions = 1;
    int _correctAnswers = 0;
    int _audioClipIndex = 0;
    string _objectName;

    [Header("Questions")]
    [SerializeField] List<QuizFormat> _questionsList;

    public int _currentQuestionIndex = 0;

    [Header("Available Buttons")]
    [SerializeField] List<Button> _buttonsList;

    [Header("Scene Game Objects")]
    [SerializeField] GameObject _stageGuideCanvas;
    [SerializeField] GameObject _imageTarget;

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
                    case "hifi":
                        PlayMusic(_audioClipIndex);
                        break;
                    default:
                        break;
                }
            }
        }

        // Below code is for testing purposes [WEBCAM]
      /*  if (Input.GetMouseButtonDown(0))
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
                            case "hifi":
                                PlayMusic(_answeredQuestions - 1);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }*/

        if (!_audioSource.isPlaying) { _musicNoteParticles.Stop(); }
    }

    void Start() => ShowQuiz();

    public void ShowQuiz()
    {
        //RandomizeButtonPosition(_buttonsList); // Randomize answer's position
        _totalQuestions = _questionsList.Count;
        GenerateQuestion();
    }

    void GenerateQuestion()
    {
        
        
        if ( _questionsList.Count > 0)
        {
            _numberOfQuestions.text = _answeredQuestions + " / " + _totalQuestions; //UI element
            //_currentQuestionIndex = Random.Range(0, _questionsList.Count); // get a random number from zero to number of our questions
            _questionText.text = _questionsList[_currentQuestionIndex].question;
            _choicesText.text = string.Empty;
            ResetButtons(true);
            //RandomizeButtonPosition(_buttonsList);
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

    void SetAnswers()
    {
        for (int i = 0; i < _buttonsList.Count; i++)
        {
            _buttonsList[i].GetComponent<Image>().color = _buttonsList[i].GetComponent<ButtonManager2>()._startColor;
            _buttonsList[i].GetComponent<ButtonManager2>()._isCorrect = false; // to avoid having all buttons to be correct
            //_buttonsList[i].transform.GetChild(0).GetComponent<Text>().text = _questionsList[_currentQuestionIndex].answers[i];
            _choicesText.text += _questionsList[_currentQuestionIndex].answers[i] + "\n";

            if (_questionsList[_currentQuestionIndex].correctAnswer == i + 1)
            {
                _buttonsList[i].GetComponent<ButtonManager2>()._isCorrect = true;
            }
        }
    }

    public void Answer(bool PlayerAnswer)
    {
        Debug.Log("This is the current question index: " + _currentQuestionIndex);
        if (PlayerAnswer == true)
            _correctAnswers++;

        _answeredQuestions++; // for every correct answer increase score by 1
        _questionsList.RemoveAt(_currentQuestionIndex); // remove answered question
        //_currentQuestionIndex++;
        _audioSource.Stop();
        _audioClipIndex++;
        ResetButtons(false);
        StartCoroutine(WaitForNext()); // Get next question
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
        stageDialogue._stageGuide.text = "Tέλος! Απαντήσατε σωστά σε " + _correctAnswers + " από τις " + _totalQuestions + " ερωτήσεις.";
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

    void PlayMusic(int audioClipIndex)
    {
        if (_audioSource.isPlaying) 
        { 
            _audioSource.Stop();
            _musicNoteParticles.Stop();
            Debug.Log("Debug message: Source and particle stop");
        }
        else {
            _audioSource.clip = _audioClips[audioClipIndex];
            _audioSource.Play();
            _musicNoteParticles.Play();

            Debug.Log("Debug message: Source and particle play");
        }
    }
}