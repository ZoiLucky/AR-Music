using UnityEngine;

public class BackButton : MonoBehaviour
{
    [SerializeField] GameObject _promptCanvas;
    [SerializeField] GameObject _stageGuide;

    void Update()
    {
        // Make sure user is on Android platform
        if (Application.platform == RuntimePlatform.Android)
        {
            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Quit the application
                if (!_stageGuide.activeSelf)
                    _promptCanvas.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (!_stageGuide.activeSelf)
                _promptCanvas.SetActive(true);
        }
    }
}