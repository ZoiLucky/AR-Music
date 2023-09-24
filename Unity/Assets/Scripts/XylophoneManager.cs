using UnityEngine;

public class XylophoneManager : MonoBehaviour
{
    public AudioClip[] _audioClips;
    public AudioSource _audioSource;
    string _buttonName;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
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
                _buttonName = hit.transform.name;
                switch (_buttonName)
                {
                    case "Do":
                        PlayMusic(_buttonName, 0);
                        break;
                    case "Re":
                        PlayMusic(_buttonName, 1);
                        break;
                    case "Mi":
                        PlayMusic(_buttonName, 2);
                        break;
                    case "Fa":
                        PlayMusic(_buttonName, 3);
                        break;
                    case "Sol":
                        PlayMusic(_buttonName, 4);
                        break;
                    case "La":
                        PlayMusic(_buttonName, 5);
                        break;
                    case "Si":
                        PlayMusic(_buttonName, 6);
                        break;
                    default:
                        break;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                //suppose i have two objects here named obj1 and obj2.. how do i select obj1 to be transformed 
                if (hit.transform != null)
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        _buttonName = hit.transform.name;
                        switch (_buttonName)
                        {
                            case "Do":
                                PlayMusic(_buttonName, 0);
                                break;
                            case "Re":
                                PlayMusic(_buttonName, 1);
                                break;
                            case "Mi":
                                PlayMusic(_buttonName, 2);
                                break;
                            case "Fa":
                                PlayMusic(_buttonName, 3);
                                break;
                            case "Sol":
                                PlayMusic(_buttonName, 4);
                                break;
                            case "La":
                                PlayMusic(_buttonName, 5);
                                break;
                            case "Si":
                                PlayMusic(_buttonName, 6);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    void PlayMusic(string buttontName, int audioClipIndex)
    {
        Debug.Log("Debug message: " + buttontName + "!!!");
        _audioSource.clip = _audioClips[audioClipIndex];
        _audioSource.Play();
    }
}