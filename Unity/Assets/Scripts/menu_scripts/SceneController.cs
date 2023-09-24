using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    [Header ("Level")] [SerializeField] string _sceneName;

    public void LoadScene() 
    {
        switch (_sceneName) 
        {
            case "SampleScene":
                SceneManager.LoadScene(_sceneName);
                break;
            case "session_1_test":
                SceneManager.LoadScene(_sceneName);
                break;
            case "session_2_test":
                SceneManager.LoadScene(_sceneName);
                break;
            case "session_3_test":
                SceneManager.LoadScene(_sceneName);
                break;
            case "session_xylo_test":
                SceneManager.LoadScene(_sceneName);
                break;                
            default:
                break;
        }
    }

    public void CloseApplication()
    {
        Application.Quit();
    }
}
