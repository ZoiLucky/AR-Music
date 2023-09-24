using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StageGuideDialogue : MonoBehaviour
{
    [Header("Text Objects")]
    //public TMP_Text _stageTitle;
    //public TMP_Text _stageGuide;
    public Text _stageTitle; // Legacy
    public Text _stageGuide; // Legacy

    [Header("Scene Game Objects")]
    [SerializeField] Transform _dialogueBox;
    [SerializeField] CanvasGroup _dialogueBackground;
    [SerializeField] GameObject _stageGuideCanvas;
    [SerializeField] GameObject _imageTarget;

    public bool _isQuizFinished = false;

/*    void Awake() 
    {
        _stageTitle.text = "Session 1 Quiz";
        _stageGuide.text = "Το Quiz αποτελείται από ερωτήσεις πολλαπλής επιλογές. Απάντησε σωστά χρησιμοποιώντας τα [A][B][C] κουμπιά.";
    }*/

    void OnEnable()
    {
        _dialogueBackground.alpha = 0;
        _dialogueBackground.LeanAlpha(1, 0.5f);

        _dialogueBox.localPosition = new Vector2(0, -Screen.height);
        _dialogueBox.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    }

    public void CloseDialog()
    {
        _dialogueBackground.LeanAlpha(0, 0.5f);
        _dialogueBox.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo();
        StartCoroutine(ToggleVisibility(_imageTarget, true, 0));

        if (_isQuizFinished == true)
        {
            LoadNextScene("main_menu");
        }
        else 
        {
            StartCoroutine(ToggleVisibility(_stageGuideCanvas, false, 10));
        }
    }

    IEnumerator ToggleVisibility(GameObject gameObject, bool status, int minutes) 
    {
        gameObject.SetActive(status);
        yield return new WaitForSeconds(minutes);
    }

    public void LoadNextScene(string sceneName) { SceneManager.LoadScene(sceneName); }
}