using System;
using Shapes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.MeshOutline
{
    [Serializable]
    public class EdgeOutlineParams
    {
    }

    public class EdgeOutlineGameObjectController : MonoBehaviour
    {


        [SerializeField]
        private Material _meshMaterial; 
        [SerializeField]
        private Line _referenceLinePrefab;

        private Line _ourOutlineLineForParams;

        public Line OutlineLineParams
        {
            get
            {
                if (_ourOutlineLineForParams)
                    _ourOutlineLineForParams = CreateAndInit(_referenceLinePrefab);
                return _ourOutlineLineForParams;
            }
            private set
            {
                if (_ourOutlineLineForParams)
                    _ourOutlineLineForParams = CreateAndInit(value);
                else
                    CopyLineParams(_ourOutlineLineForParams,value);
            }
        }

        private Line CreateAndInit(Line refLine)
        {
            var ourLine = gameObject.AddComponent<Line>();

            CopyLineParams(ourLine, refLine);

            return ourLine;

        }

        private static void CopyLineParams(Line toLine, Line fromLine)
        {
            toLine.Thickness = fromLine.Thickness;

            toLine.Color = fromLine.Color;
            toLine.ColorMode = fromLine.ColorMode;
            toLine.ColorStart = fromLine.ColorStart;
            toLine.ColorEnd = fromLine.ColorEnd;

            toLine.Dashed = fromLine.Dashed;
            toLine.DashType = fromLine.DashType;
            toLine.DashSnap = fromLine.DashSnap;
            toLine.DashSize = fromLine.DashSize;
            toLine.DashOffset = fromLine.DashOffset;
        }

        private EdgeOutlineMeshComponent[] _childrenMeshOutlineComponents;

        //todo: Create Back processor that goes through all child components with Edge Outline.
        //todo: For each of those child MeshFilters it creates Shapes line Controller and add lines for it if not yet existing.
        //todo: Register them incide parent class so we can contorol multiple parameters. 
        /* Parameters to be control
            1. Mesh Material
            2. Line thickness
            3. Line type
            4. Line color 
            5. Parameters related to main camera 
        */


        [SerializeField]
        private bool _redrawOutline =true;
        
        private void OnDrawGizmosSelected()
        {
            Debug.Log($"Edge Outline Game Object drawing Gizmos");

            if (_redrawOutline)
            {
                ScanAndProcessMeshFilterInChildren();
                _redrawOutline = false;
            }
        }

        private void ScanAndProcessMeshFilterInChildren()
        {
           var meshes =  gameObject.GetComponentsInChildren<MeshFilter>();
           for (int i = 0; i < meshes.Length; i++)
           {
                //todo: remove
               Debug.Log($"MF: {i}. {meshes[i].name} vertices count {meshes[i].sharedMesh.vertices.Length}");

               var go = meshes[i].gameObject;
               var outlineComponent = go.GetComponent<EdgeOutlineMeshComponent>();
               if (outlineComponent == null)
               {
                   outlineComponent = go.AddComponent<EdgeOutlineMeshComponent>();
               }

               outlineComponent.InitilizeOutline(this,_referenceLinePrefab); 

           }
        }
    }
}