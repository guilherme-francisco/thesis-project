using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MixedVirtualManager : MonoBehaviour
{
    [SerializeField] private GameObject aRSession;
    [SerializeField] private GameObject xrCamera;
    [SerializeField] private GameObject floor;

    private Color solidColor = new(0, 0, 0, 0);

    public void OnSliderChanged(float value)
    {
        Camera cam = xrCamera.GetComponent<Camera>();
        ARCameraBackground aRCameraBackground = xrCamera.GetComponent<ARCameraBackground>();
        ARCameraManager aRCameraManager = xrCamera.GetComponent<ARCameraManager>();
        if (value == 1)
        {
            //aRSession.SetActive(true);
            aRCameraBackground.enabled = true;
            aRCameraManager.enabled = true;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = solidColor;
            floor.SetActive(false);
        }
        else
        {
            // aRSession.SetActive(false);
            aRCameraBackground.enabled = false;
            aRCameraManager.enabled = false;
            cam.clearFlags = CameraClearFlags.Skybox;
            floor.SetActive(true);
        }

    }
}
