using System.Collections;
using UnityEngine;

public class ContainerAnimation : MonoBehaviour
{
    public Transform box;

    public void Awake()
    {
        box.localPosition = new Vector2(0.5f, -Screen.height);
    }

    public void PopupAnimation()
    {
        box.localPosition = new Vector2(0, -Screen.height);
        box.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    }

    public void CloseDialog()
    {
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo();
    }
}
