using System;
using UnityEngine;

namespace Assets
{
    public class CameraEyeComponent:MonoBehaviour
    {

        public RenderTexture TextureR; 
        public RenderTexture TextureL;
        public Camera Cam;


        private void Awake()
        {
            Cam = GetComponent<Camera>();
        } 
        public void SetTextures(RenderTexture l, RenderTexture r)
        {
            TextureL = l;
            TextureR = r;

        }


        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            var  monoOrStereoscopicEye = Cam.stereoActiveEye;

         if (monoOrStereoscopicEye == Camera.MonoOrStereoscopicEye.Left)
        {
            Graphics.Blit(src,TextureL);
        }
        else if (monoOrStereoscopicEye == Camera.MonoOrStereoscopicEye.Right)
        { 
            Graphics.Blit(src,TextureR);
        }
         
        }
    }
}