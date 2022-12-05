// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using VertexFragment;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos
{
    public class SliderChangeOutline : MonoBehaviour
    {
        
        
        [SerializeField]
        private Renderer _postProcessVolume;

        [SerializeField]
        private Material _material; 

        public void OnSliderUpdatedRed(SliderEventData eventData) 
        { 

            if ((_material != null) )
            {
                Color color = _material.GetColor("_RimColor");
                _material.SetColor("_RimColor", new Color(eventData.NewValue, color.g, color.b));
            }
        }

        public void OnSliderUpdatedGreen(SliderEventData eventData)
        {
            if ((_material != null) )
            {
                _material.SetFloat("_RimPower",eventData.NewValue*8);

            }
        }

        public void OnSliderUpdateBlue(SliderEventData eventData)
        {
            if ((_material != null) )
            {
                _material.color = new Color(eventData.NewValue,eventData.NewValue,eventData.NewValue );
            }
        }
       
    }
}
