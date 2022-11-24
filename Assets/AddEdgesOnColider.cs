using System;
using System.Collections;
using System.Collections.Generic;
using Plawius.NonConvexCollider;
using Shapes;
using UnityEngine;

[RequireComponent(typeof(NonConvexColliderComponent))]
[RequireComponent(typeof(Polyline))]
public class AddEdgesOnColider : MonoBehaviour
{
    private  PolylinePoint _polylineReference = new PolylinePoint(Vector2.zero,Color.white,2f);
        
    private NonConvexColliderComponent _nonConvexColliderComponent;
    private Polyline _polyline;

    private void OnDrawGizmosSelected()
    {
        Debug.Log($"Hello world");


        if (_nonConvexColliderComponent == null)
            _nonConvexColliderComponent = GetComponent<NonConvexColliderComponent>();
        if (_polyline == null)
            _polyline = GetComponent<Polyline>();

        _polyline.points = new List<PolylinePoint>();

        var colliders = _nonConvexColliderComponent.ConvexColliders;
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
    }
}

