using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PromptAnimation : MonoBehaviour
{
    [SerializeField] LeanTweenType _inType;
    [SerializeField] LeanTweenType _outType;
    [SerializeField] UnityEvent _onCompleteCallback;
    string _sceneName;

    public GameObject _promptContainer;

    public void SetSceneName(string sceneName) 
    { 
        this._sceneName = sceneName;
        Debug.Log(_sceneName);
    }

    public void OnEnable() 
    {
        _promptContainer.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(_promptContainer, new Vector3(1, 1, 1), 0.25f).setDelay(0.1f).setOnComplete(onComplete).setEase(_inType);
    }

    void onComplete() 
    {
        if (_onCompleteCallback != null)
        {
            _onCompleteCallback.Invoke();
        }
    }

    public void LoadScene() 
    {
        Debug.Log(_sceneName);
        if (_sceneName == null) { _sceneName = "main_menu"; }
        SceneManager.LoadScene(_sceneName);
    }

    public void OnClose() 
    {
        LeanTween.scale(_promptContainer, new Vector3(0, 0, 0), 0.25f).setEase(_outType);
    }
}
