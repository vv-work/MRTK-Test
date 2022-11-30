using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ShowAllEdge : MonoBehaviour
{

    [SerializeField]
    [Range(0,5f)] 
    private float _rayDistance=0.2f;
    [SerializeField]
    [Range(0,5f)] 
    private float _radius = 0.01f;

    private MeshFilter _meshFilter;

    private void OnDrawGizmos()
    {
        if (_meshFilter == null)
            _meshFilter = GetComponent<MeshFilter>();


        var vertices = _meshFilter.mesh.vertices;
        var normals = _meshFilter.mesh.normals;

        if (vertices == null)
        {
            return;
        }
        for (int i = 0; i < vertices.Length; i++)
        {
            var pos = vertices[i]+transform.position;
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(pos, _radius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(pos, normals[i].normalized*_rayDistance);
        }
    }
}
