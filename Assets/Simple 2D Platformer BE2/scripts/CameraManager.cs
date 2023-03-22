using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera DynamicCamera;
    public Camera StaticCamera;

    private void Awake()
    {
        ShowDC();
    }

    private void Update()
    {
        if (Input.GetButton("q"))
        {
            ShowSC();
        }
        if (Input.GetButton("e"))
        {
            ShowDC();
        }

    }
    public void ShowDC()
    {
        DynamicCamera.enabled = true;
        StaticCamera.enabled = false;
    }
    public void ShowSC()
    {
        DynamicCamera.enabled = false;
        StaticCamera.enabled = true;
    }
}
