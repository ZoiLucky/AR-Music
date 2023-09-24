using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerPiano : MonoBehaviour
{
    [Header("Audio Objects")]
    [SerializeField] AudioClip[] _audioClips;
    [SerializeField] AudioSource _audioSource;

    [Header("UI Element")]
    [SerializeField] ParticleSystem[] _particleSystems;

    void Update()
    {
        if ((Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != null)
                {
                    string objectName = hit.transform.name;
                    switch (objectName)
                    {
                        case "hifi":
                            PlayMusic(0, _particleSystems[0]);
                            break;
                        case "piano":
                            PlayMusic(1, _particleSystems[1]);
                            break;
                        case "stars":
                            PlayMusic(0, _particleSystems[0]);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        if (!_audioSource.isPlaying)
        {
            foreach (ParticleSystem particleSystem in _particleSystems) { particleSystem.Stop(); }
        }
    }

    void PlayMusic(int audioClipIndex, ParticleSystem particleSystem)
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
            particleSystem.Stop();
        }
        else
        {
            _audioSource.clip = _audioClips[audioClipIndex];
            _audioSource.Play();
            particleSystem.Play();
        }
    }
}