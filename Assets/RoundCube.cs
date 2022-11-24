using System;
using UnityEngine;
using System.Collections;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos
{
    [UnityEngine.RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class RoundCube : MonoBehaviour
    {


        public int xSize, ySize, zSize;

        private Mesh mesh;
        private Vector3[] vertices;
        private Coroutine _cycle; 

        private void Awake()
        {
            Cycle();
        }
        private void OnDrawGizmosSelected()
        {
            Cycle();
        }

        private void Cycle()
        {
            if (_cycle != null)
            {
                _cycle = StartCoroutine(Generate());
            }
        }


        private IEnumerator Generate()
        {
            yield return GenerateCube();    
        }

        private IEnumerator GenerateCube()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Procedural Cube";
            WaitForSeconds wait = new WaitForSeconds(0.05f);

            int cornerVertices = 8;
            int edgeVertices = (xSize + ySize + zSize - 3) * 4;
            int faceVertices = (
                (xSize - 1) * (ySize - 1) +
                (xSize - 1) * (zSize - 1) +
                (ySize - 1) * (zSize - 1)) * 2;
            vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
            int v = 0;
            for (int y = 0; y <= ySize; y++)
            {

                for (int x = 0; x <= xSize; x++)
                {
                    vertices[v++] = new Vector3(x, y, 0);

                }

                for (int z = 1; z <= zSize; z++)
                {
                    vertices[v++] = new Vector3(xSize, y, z);
                    yield return wait;
                }

                for (int x = xSize - 1; x >= 0; x--)
                {
                    vertices[v++] = new Vector3(x, 0, zSize);
                    yield return wait;
                }

                for (int z = zSize - 1; z > 0; z--)
                {
                    vertices[v++] = new Vector3(0, 0, z);
                    yield return wait;
                }
            }


            yield return wait;
        }

        private void OnDrawGizmos()
        {
            if (vertices == null)
            {
                return;
            }

            Gizmos.color = Color.black;
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }
    }
} 
