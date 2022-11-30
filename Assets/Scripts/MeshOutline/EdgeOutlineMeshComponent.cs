using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Shapes;
using UnityEngine;

namespace Assets.Scripts.MeshOutline
{
    [RequireComponent(typeof(MeshFilter))]
    public class EdgeOutlineMeshComponent : MonoBehaviour
    {

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

        private bool _generateCube;
        private Mesh mesh;
        private Vector3[] vertices;
        private Coroutine _cycle;

        [Range(0.0001f, 0.1f)]
        [SerializeField]
        private float delays = 0.05f;

        private Mesh _mesh;
        private Edge[] _edges;

        [SerializeField]
        private Line[] _shapeLines;

        private Line[] _edgeLines;


        private void OnDrawGizmosSelected()
        {

            if(_mesh==null)
                _mesh = GetComponent<MeshFilter>().sharedMesh;
            

            if (_edges == null)
                _edges =  BuildManifoldEdges(_mesh);

            var v= _mesh.vertices;
            var pos=  transform.position; 
            for (int i = 0; i < _edges.Length; i++)
            {
                var edge = _edges[i];
                var pointA = v[edge.vertexIndex[0]];
                var pointB = v[edge.vertexIndex[1]];

                Gizmos.DrawLine(pointA+pos,pointB+pos);
            }
        }


        /// Builds an array of edges that connect to only one triangle.
     /// In other words, the outline of the mesh    
     public static Edge[] BuildManifoldEdges(Mesh mesh)
     {
         // Build a edge list for all unique edges in the mesh
         Edge[] edges = BuildEdges(mesh.vertexCount, mesh.triangles);
 
         // We only want edges that connect to a single triangle
         ArrayList culledEdges = new ArrayList();
         foreach (Edge edge in edges)
         {
             if (edge.faceIndex[0] == edge.faceIndex[1])
             {
                 culledEdges.Add(edge);
             }
         }
 
         return culledEdges.ToArray(typeof(Edge)) as Edge[];
     }
 
     /// Builds an array of unique edges
     /// This requires that your mesh has all vertices welded. However on import, Unity has to split
     /// vertices at uv seams and normal seams. Thus for a mesh with seams in your mesh you
     /// will get two edges adjoining one triangle.
     /// Often this is not a problem but you can fix it by welding vertices 
     /// and passing in the triangle array of the welded vertices.
     public static Edge[] BuildEdges(int vertexCount, int[] triangleArray)
     {
         int maxEdgeCount = triangleArray.Length;
         int[] firstEdge = new int[vertexCount + maxEdgeCount];
         int nextEdge = vertexCount;
         int triangleCount = triangleArray.Length / 3;
 
         for (int a = 0; a < vertexCount; a++)
             firstEdge[a] = -1;
 
         // First pass over all triangles. This finds all the edges satisfying the
         // condition that the first vertex index is less than the second vertex index
         // when the direction from the first vertex to the second vertex represents
         // a counterclockwise winding around the triangle to which the edge belongs.
         // For each edge found, the edge index is stored in a linked list of edges
         // belonging to the lower-numbered vertex index i. This allows us to quickly
         // find an edge in the second pass whose higher-numbered vertex index is i.
         Edge[] edgeArray = new Edge[maxEdgeCount];
 
         int edgeCount = 0;
         for (int a = 0; a < triangleCount; a++)
         {
             int i1 = triangleArray[a * 3 + 2];
             for (int b = 0; b < 3; b++)
             {
                 int i2 = triangleArray[a * 3 + b];
                 if (i1 < i2)
                 {
                     Edge newEdge = new Edge();
                     newEdge.vertexIndex[0] = i1;
                     newEdge.vertexIndex[1] = i2;
                     newEdge.faceIndex[0] = a;
                     newEdge.faceIndex[1] = a;
                     edgeArray[edgeCount] = newEdge;
 
                     int edgeIndex = firstEdge[i1];
                     if (edgeIndex == -1)
                     {
                         firstEdge[i1] = edgeCount;
                     }
                     else
                     {
                         while (true)
                         {
                             int index = firstEdge[nextEdge + edgeIndex];
                             if (index == -1)
                             {
                                 firstEdge[nextEdge + edgeIndex] = edgeCount;
                                 break;
                             }
 
                             edgeIndex = index;
                         }
                     }
 
                     firstEdge[nextEdge + edgeCount] = -1;
                     edgeCount++;
                 }
 
                 i1 = i2;
             }
         }
 
         // Second pass over all triangles. This finds all the edges satisfying the
         // condition that the first vertex index is greater than the second vertex index
         // when the direction from the first vertex to the second vertex represents
         // a counterclockwise winding around the triangle to which the edge belongs.
         // For each of these edges, the same edge should have already been found in
         // the first pass for a different triangle. Of course we might have edges with only one triangle
         // in that case we just add the edge here
         // So we search the list of edges
         // for the higher-numbered vertex index for the matching edge and fill in the
         // second triangle index. The maximum number of comparisons in this search for
         // any vertex is the number of edges having that vertex as an endpoint.
 
         for (int a = 0; a < triangleCount; a++)
         {
             int i1 = triangleArray[a * 3 + 2];
             for (int b = 0; b < 3; b++)
             {
                 int i2 = triangleArray[a * 3 + b];
                 if (i1 > i2)
                 {
                     bool foundEdge = false;
                     for (int edgeIndex = firstEdge[i2]; edgeIndex != -1; edgeIndex = firstEdge[nextEdge + edgeIndex])
                     {
                         Edge edge = edgeArray[edgeIndex];
                         if ((edge.vertexIndex[1] == i1) && (edge.faceIndex[0] == edge.faceIndex[1]))
                         {
                             edgeArray[edgeIndex].faceIndex[1] = a;
                             foundEdge = true;
                             break;
                         }
                     }
 
                     if (!foundEdge)
                     {
                         Edge newEdge = new Edge();
                         newEdge.vertexIndex[0] = i1;
                         newEdge.vertexIndex[1] = i2;
                         newEdge.faceIndex[0] = a;
                         newEdge.faceIndex[1] = a;
                         edgeArray[edgeCount] = newEdge;
                         edgeCount++;
                     }
                 }
 
                 i1 = i2;
             }
         }
 
         Edge[] compactedEdges = new Edge[edgeCount];
         for (int e = 0; e < edgeCount; e++)
             compactedEdges[e] = edgeArray[e];
 
         return compactedEdges;
     }

     public void InitilizeOutline(EdgeOutlineGameObjectController controller,Line outlineLineParams)
     {

         // Cleaning if there was lines before now it's time to remove them
         for (int i = 0; i < _shapeLines?.Length; i++)
         {
                var line = _shapeLines[i];
                if(line!=null)
                    Destroy(line); 
         }

            if(_mesh==null)
                _mesh = GetComponent<MeshFilter>().sharedMesh;
            

            if (_edges == null)
                _edges =  BuildManifoldEdges(_mesh);

            var v= _mesh.vertices;
            var pos=  transform.position;

            if (_edgeLines != null)
            {
                for (int i = 0; i < _edgeLines?.Length; i++)

                    if(_edgeLines[i]!=null)
                        Destroy(_edgeLines[i].gameObject);
            }

            _edgeLines = new Line[_edges.Length];

            for (int i = 0; i < _edges.Length; i++)
            {
                Line line = Instantiate(outlineLineParams,transform);
                var edge = _edges[i];
                var pointA = v[edge.vertexIndex[0]];
                var pointB = v[edge.vertexIndex[1]];


                line.transform.position = transform.position;
                line.transform.rotation = transform.rotation;
                line.Start = pointA;
                line.End = pointB;

                Gizmos.DrawLine(pointA+pos,pointB+pos);
            }
     }
    }
 
    }
