using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using UnityEngine.ProBuilder;

[RequireComponent(typeof(Polyline))]
public class AddEdgesOnColider : MonoBehaviour
{
    private  PolylinePoint _polylineReference = new PolylinePoint(Vector2.zero,Color.white,2f);
        
    private Polyline _polyline;

    private void OnDrawGizmosSelected()
    {
        /*
        Debug.Log($"Hello world");


        if (_polyline == null)
            _polyline = GetComponent<Polyline>();

        _polyline.points = new List<PolylinePoint>();

        Dictionary<int, Vector3> lines = new Dictionary<int, Vector3>();

        for (int i = 0; i < colliders.Count; i++)
        {
            var localMesh = colliders[i].sharedMesh;
            for (int j = 0; j < localMesh.vertices.Length; j++)
            {
                var value = localMesh.vertices[i];

                var clone = _polylineReference;

                clone.point = value;

                _polyline.points.Add(clone);
            }


        }
        */
    }
}

public struct Triangle
{
    public Vector3 A { get; private set; }
    public Vector3 B { get; private set; }
    public Vector3 C { get; private set; }

}
