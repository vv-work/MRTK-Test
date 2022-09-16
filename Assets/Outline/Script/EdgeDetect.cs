using UnityEngine;
using TMPro;
using System;
using Assets;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UnityEngine.Camera.MonoOrStereoscopicEye;

[Serializable]
public class CameraSet
{
    private EdgeDetect _edgeDetect;
    public  RenderTexture depthTexture;
    public RenderTexture normalsTexture;


    public Material edgeDetectMat;

    public RenderTexture edgeTexture;
    public Material edgeCombineMat;
        
    private Camera _depthCaptureCamera;
    private Camera _normalCaptureCamera;


    public void InitCameras(EdgeDetect edgeDetect, Camera.MonoOrStereoscopicEye activeEye, Camera depthCaptureCamera, Camera normalCaptureCamera)
    {
        _edgeDetect = edgeDetect; 

        _depthCaptureCamera = depthCaptureCamera;
        _depthCaptureCamera.projectionMatrix = _edgeDetect.cam.projectionMatrix;
        _normalCaptureCamera = normalCaptureCamera;
        _normalCaptureCamera.projectionMatrix = _edgeDetect.cam.projectionMatrix;

        depthTexture = new RenderTexture(_edgeDetect.width, _edgeDetect.height, 24, RenderTextureFormat.ARGB32);
        depthTexture.name = $"Depth {activeEye}";
        normalsTexture = new RenderTexture(_edgeDetect.width, _edgeDetect.height, 24, RenderTextureFormat.ARGB32);
        normalsTexture.name = $"normals {activeEye}";


        depthTexture.filterMode = FilterMode.Point;
        normalsTexture.filterMode = FilterMode.Point;

        edgeTexture = new RenderTexture(_edgeDetect.width, _edgeDetect.height, 0, RenderTextureFormat.ARGB32); //just need boolean
        edgeTexture.filterMode = FilterMode.Point; 

        _normalCaptureCamera.ResetProjectionMatrix();

        _normalCaptureCamera.gameObject.SetActive(true);
        _normalCaptureCamera.SetReplacementShader(_edgeDetect.normalsShader, "RenderType");

        edgeDetectMat = new Material(_edgeDetect.edgeDetectShader);
        edgeDetectMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        edgeDetectMat.SetTexture("_NormalsTex", normalsTexture);
        edgeDetectMat.SetVector("_SensitivityAndWidthArgs", new Vector4(_edgeDetect.depthSensitivity, _edgeDetect.normalsSensitivity, 0, 0));
        _edgeDetect.prevNormalsSensitivity = _edgeDetect.normalsSensitivity;
        _edgeDetect.prevDepthSensitivity = _edgeDetect.depthSensitivity;

        Shader.SetGlobalInt("_EdgedetectDebugMode", (int)_edgeDetect.debugMode);
        _edgeDetect.prevDebugMode = _edgeDetect.debugMode;

         edgeCombineMat = new Material(_edgeDetect.edgeCombineShader);
        edgeCombineMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        edgeCombineMat.SetTexture("_EdgeTex", edgeTexture);
        edgeCombineMat.color = _edgeDetect.outlineColor;
        _edgeDetect.prevOutlineColor = _edgeDetect.outlineColor;

        _depthCaptureCamera.SetReplacementShader(_edgeDetect.encodedDepthShader, "RenderType");
        // _depthCaptureCamera.targetTexture = depthTexture;

        _depthCaptureCamera.ResetProjectionMatrix();
        _depthCaptureCamera.gameObject.SetActive(true);
    }

    public void SetupCameras()
    {
        Vector3 a = -_edgeDetect.cam.transform.InverseTransformPoint(_edgeDetect.cam.ScreenToWorldPoint(Vector3.forward));
        a.z = _edgeDetect.cam.farClipPlane * EdgeDetect.farClipPlaneMultiplicand;

        edgeDetectMat.SetMatrix("_Cam2World", Matrix4x4.TRS(Vector3.zero, _edgeDetect.cam.transform.rotation, Vector3.one));
        Shader.SetGlobalVector("_EdgeDetectDepthArgs", a);

        _normalCaptureCamera.fieldOfView = _depthCaptureCamera.fieldOfView = _edgeDetect.cam.fieldOfView;
        _normalCaptureCamera.farClipPlane = _depthCaptureCamera.farClipPlane = _edgeDetect.cam.farClipPlane;
        _normalCaptureCamera.nearClipPlane = _depthCaptureCamera.nearClipPlane = _edgeDetect.cam.nearClipPlane;


        if (_edgeDetect.prevNormalsSensitivity != _edgeDetect.normalsSensitivity || _edgeDetect.prevDepthSensitivity != _edgeDetect.depthSensitivity || _edgeDetect.prevDepth2Sensitivity != _edgeDetect.depthSensitivity2)
        {
            edgeDetectMat.SetVector("_SensitivityAndWidthArgs",
                new Vector4(_edgeDetect.normalsSensitivity, _edgeDetect.depthSensitivity, _edgeDetect.depthSensitivity2, 0));
            _edgeDetect.prevNormalsSensitivity = _edgeDetect.normalsSensitivity;
            _edgeDetect.prevDepthSensitivity = _edgeDetect.depthSensitivity;
            _edgeDetect.prevDepth2Sensitivity = _edgeDetect.depthSensitivity2;
        }

        if (_edgeDetect.debugMode != _edgeDetect.prevDebugMode)
        {
            Shader.SetGlobalInt("_EdgedetectDebugMode", (int)_edgeDetect.debugMode);
            _edgeDetect.prevDebugMode = _edgeDetect.debugMode;
        }

        if (_edgeDetect.prevOutlineColor != _edgeDetect.outlineColor)
        {
            edgeCombineMat.color = _edgeDetect.outlineColor;
            _edgeDetect.prevOutlineColor = _edgeDetect.outlineColor;
        }
    }
}

public class EdgeDetect : MonoBehaviour {


    [Range(0f, 10f)]
    public float depthSensitivity = 1;

    public float prevDepthSensitivity;
    [Range(0f, 10f)]
    public float depthSensitivity2 = 1;

    public float prevDepth2Sensitivity;
    [Range(0f, 10f)]
    public float normalsSensitivity = 1;

    public float prevNormalsSensitivity;

    public DebugMode debugMode = DebugMode.none;
    public DebugMode prevDebugMode;
    

    public Color outlineColor = Color.black;
    public Color prevOutlineColor;

    public const float farClipPlaneMultiplicand = 1.75f;

    public Camera cam;

    [HideInInspector]
    public Shader edgeDetectShader;
    [HideInInspector]
    public Shader edgeCombineShader;
    [HideInInspector]
    public Shader normalsShader;
    [HideInInspector]
    public Shader encodedDepthShader;



    public int width = -1;
    public int height = -1;


    public CameraEyeComponent depthCam; 
    public CameraEyeComponent normalCam;

    public CameraSet CameraSetL;
    public CameraSet CameraSetR;


    void LateUpdate() {

        if (cam == null && width != -1)
        {

            cam = GetComponent<Camera>();

            CameraSetL.InitCameras(this,Left, depthCam.Cam, normalCam.Cam);
            CameraSetR.InitCameras(this,Right, depthCam.Cam, normalCam.Cam); 
            depthCam.SetTextures(CameraSetL.depthTexture,CameraSetR.depthTexture);
            normalCam.SetTextures(CameraSetL.normalsTexture,CameraSetR.normalsTexture);
            
        } 

        if (cam != null)
        {
            CameraSetL.SetupCameras();
            CameraSetR.SetupCameras();
        }
      
    }


    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        
        if (cam == null) {
            Graphics.Blit(source, destination);
            width = source.width;
            height = source.height;
        } else {
            if (cam.stereoActiveEye == Left)
            { 
                Graphics.Blit(CameraSetL.depthTexture, CameraSetL.edgeTexture, CameraSetL.edgeDetectMat);
                Graphics.Blit(source, destination, CameraSetL.edgeCombineMat);
            }
            if (cam.stereoActiveEye == Right)
            { 

                Graphics.Blit(CameraSetR.depthTexture, CameraSetR.edgeTexture, CameraSetR.edgeDetectMat);
               Graphics.Blit(source, destination, CameraSetR.edgeCombineMat);
            }
        }
        
    }

    public enum DebugMode {
        none,
        outlines,
        normals,
        simpleDepth,
        depthCompression,
        worldSpace
    }
} 
