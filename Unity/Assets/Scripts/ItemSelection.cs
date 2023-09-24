using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ItemSelection : MonoBehaviour
{
    [Header ("Buttons")]
    [SerializeField] Button _leftArrow;
    [SerializeField] Button _rightArrow;

    [Header ("Other")]
    [SerializeField] int _activeItem;

    string _objectName;

    [Header("Text Settings")]
    [SerializeField] [TextArea] string[] _instrumentInfo;
    [SerializeField] float _textSpeed = 0.01f;

    [Header("UI Elements")]
    //[SerializeField] TextMeshProUGUI _instrumentInfoText;
    [SerializeField] Text _instrumentInfoText; // Legacy
    int _currentDisplayingText = 0;

    [SerializeField] GameObject _scrollingTextGameObject;

    [SerializeField]
    List<string> _objectNamesList = new List<string> { "mozart", "debussy", "chopin", "beethoven", "bach", "vivaldi", "piano"
    , "drums", "violin", "cello", "laouto", "pontiaki_lura", "kritiki_lura", "guitar", "electricguitar", "surigga_tou_panos", "trumpet", "flute", "saxophone", "clarinet" };

    [Header("Audio Objects")]
    [SerializeField] AudioClip[] _audioClips;
    [SerializeField] AudioSource _audioSource;

    void SelectItem(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == index);
        }
    }

    public void ChangeItem(int _change)
    {
        _scrollingTextGameObject.SetActive(false);
        _activeItem += _change;

        if (_activeItem > transform.childCount - 1)
            _activeItem = 0;
        else if (_activeItem < 0)
            _activeItem = transform.childCount - 1;

        SelectItem(_activeItem);
        _audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _objectName = hit.transform.name;
                foreach (string name in _objectNamesList)
                {
                    if (name.Equals(_objectName))
                        ActivateText(_activeItem);
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
                        foreach (string name in _objectNamesList) 
                        {
                            if (name.Equals(_objectName))
                                ActivateText(_activeItem);
                        }
                    }
                }
            }
        }
    }

    public void CloseDialog() 
    { 
        _scrollingTextGameObject.SetActive(false);
        _audioSource.Stop();
    }

    void ActivateText(int index) 
    {
        _instrumentInfoText.text = string.Empty;
        _currentDisplayingText = index;
        _scrollingTextGameObject.SetActive(true);
        _instrumentInfoText.text = _instrumentInfo[_currentDisplayingText];
        //StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        for (int i = 0; i < _instrumentInfo[_currentDisplayingText].Length + 1; i++)
        {
            _instrumentInfoText.text = _instrumentInfo[_currentDisplayingText].Substring(0, i);
            yield return new WaitForSeconds(_textSpeed);
        }
    }
    public void PlayMusic()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
            Debug.Log("Debug message: Source and particle stop");
        }
        else
        {
            _audioSource.clip = _audioClips[_activeItem];
            _audioSource.Play();

            Debug.Log("Debug message: Source and particle play");
        }
    }
}