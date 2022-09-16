using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEyeComponent : MonoBehaviour
{
    private void Awake()
    {

        if (_cam == null) 
            _cam = GetComponent<Camera>();
        /*
        _cam.stereoConvergence = 0.11f;


        Matrix4x4 viewL = _cam.worldToCameraMatrix;
        Matrix4x4 viewR = _cam.worldToCameraMatrix;

        viewL[12] += 0.011f;
        viewR[12] -= 0.011f;
        _cam.SetStereoViewMatrix(Camera.StereoscopicEye.Left, viewL);
        _cam.SetStereoViewMatrix(Camera.StereoscopicEye.Right, viewR);
        */
        _cam.ResetStereoProjectionMatrices();
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (_cam == null)
            _cam = GetComponent<Camera>();
        Debug.Log($"On {gameObject.name} Active eye is {_cam.stereoActiveEye}");

        
        
    }

    private Camera _cam;
}
