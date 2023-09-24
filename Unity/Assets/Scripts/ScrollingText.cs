using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScrollingText : MonoBehaviour
{
    [Header ("Text Settings")]
    [SerializeField] [TextArea] string[] _instrumentInfo;
    [SerializeField] float _textSpeed = 0.01f;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI _instrumentInfoText;
    int _currentDisplayingText = 0;

    [SerializeField] GameObject _scrollingTextGameObject;

    private void OnEnable()
    {
        StartCoroutine(AnimateText());
    }

    public void ActivateText() 
    {
        //StartCoroutine(AnimateText());
        _scrollingTextGameObject.SetActive(false);
    }

    IEnumerator AnimateText() 
    {
        for (int i = 0; i < _instrumentInfo[_currentDisplayingText].Length + 1; i++) 
        {
            _instrumentInfoText.text = _instrumentInfo[_currentDisplayingText].Substring(0, i);
            yield return new WaitForSeconds(_textSpeed);
        }
    }
}
