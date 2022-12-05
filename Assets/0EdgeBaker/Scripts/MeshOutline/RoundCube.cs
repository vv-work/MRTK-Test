using System.Collections;
using Shapes;
using UnityEngine;

namespace Assets.Scripts.MeshOutline
{
    [UnityEngine.RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class RoundCube : MonoBehaviour
    {


        [SerializeField]
        private Transform _camera; 
        [SerializeField]
        private Line[] _myLines;

        [SerializeField]
        private float _maxDistance= 5f;

        [SerializeField]
        private float _minDistance = 0.1f;

        [SerializeField]
        private float _maxThickness=2f;
        [SerializeField]
        private float _minThickness=0.1f;

        public int xSize, ySize, zSize;

        [SerializeField]
        private bool _generateCubeOrMesh;

        private Mesh _originalMash;

    
        private bool _generateCube;
        private Mesh mesh;
        private Vector3[] vertices;
        private Coroutine _cycle;

        [Range(0.0001f, 0.1f)]
        [SerializeField]
        private float delays = 0.05f;

        private void Start()
        {
            if (_camera == null)
                _camera = Camera.main.transform;

        }

        private IEnumerator Generate()
        {
            yield break;
            if (_originalMash == null)
                _originalMash = GetComponent<MeshFilter>().mesh;

            if (_generateCube)
                yield return GenerateCube();
            else
                yield return GenerateMesh();
        }

        private IEnumerator GenerateMesh()
        {
            while (true)
            {
                WaitForSeconds wait = new WaitForSeconds(delays);
                

                vertices = new Vector3[_originalMash.vertexCount + 1];
                int v = 0;
                for (int i = 0; i < _originalMash.vertexCount; i++)
                {
                    vertices[i++] = _originalMash.vertices[i];
                    vertices[i++] = _originalMash.vertices[i];
                }


                yield return wait;
                _cycle = null;
                yield break;
            }
        }

        private void Update()
        {
            if (_camera == null || _myLines == null)
                return;
                
            foreach (Line myLine in _myLines)
            {
                float distance = Vector3.Cross(_camera.position, transform.position).sqrMagnitude/_maxDistance;
                Debug.Log($"Sqr Dist {distance}");
                float f= Mathf.Lerp(_minDistance,_maxDistance,distance)/_maxDistance;



                float thickness = (Mathf.Lerp(_minThickness, _maxThickness, f)); 
                //Debug.Log($"{thickness} Set thickness to debug build");

                myLine.Thickness = thickness;

            }

        }

        private IEnumerator GenerateCube()
        {
            while (true)
            {
               // GetComponent<MeshFilter>().mesh = mesh = new Mesh();
                //mesh.name = "Procedural Cube";
                WaitForSeconds wait = new WaitForSeconds(delays);

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
                _cycle = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            return;
            if (_generateCube != _generateCubeOrMesh)
            {
                StopCoroutine(Generate());
                vertices = null;
                _generateCube = _generateCubeOrMesh;
                _cycle = null;
            }

            if (_cycle == null)
            {
                _cycle = StartCoroutine(Generate());
            }

            if (vertices == null)
            {
                return;
            }

            Gizmos.color = Color.black;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices == null)
                    break;
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }

    }
} 
