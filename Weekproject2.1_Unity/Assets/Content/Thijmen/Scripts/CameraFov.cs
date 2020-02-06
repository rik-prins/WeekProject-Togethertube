using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFov : MonoBehaviour
{
    private Camera playerCam;
    private float targetFov;
    private float fov;

    private void Awake()
    {
        playerCam = GetComponent<Camera>();
        targetFov = playerCam.fieldOfView;
        fov = targetFov;
    }

    private void Update()
    {
        float fovSpeed = 4f;
        fov = Mathf.Lerp(fov, targetFov, Time.deltaTime * fovSpeed);
        playerCam.fieldOfView = fov;
    }

    public void SetCameraFov(float targetFov)
    {
        this.targetFov = targetFov;
    }
}