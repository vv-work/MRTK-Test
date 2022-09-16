using UnityEngine;

public class CameraEyeComponent : MonoBehaviour
{


    public RenderTexture MainTexture;
 
    private void OnEnable()
    {

        if (_cam == null) 
            _cam = GetComponent<Camera>();
        //_cam.ResetStereoProjectionMatrices();
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (_cam == null)
            _cam = GetComponent<Camera>();
        Debug.Log($"On {gameObject.name} Active eye is {_cam.stereoActiveEye}");
        if (MainTexture != null)
            Graphics.Blit(src, MainTexture);


        // Graphics.Blit(src,_cam.targetTexture);

    }

    private Camera _cam;
}
