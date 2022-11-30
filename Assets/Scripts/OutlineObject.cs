using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
 //TODO Clean up stuff. Fix bug and tweak stuff. Optimize stuff. Animation support. Add climbing support. Make usable for non-perfect quads. Find all edges within reach instead of just the closest.

[RequireComponent(typeof(MeshFilter))]
public class OutlineObject : MonoBehaviour
{
    public float reach = 3;    //TODO Check from players arms, legs etc
    private Vector3[] quad;
    private Vector3[] top;
    void Start()
    {
        quad = new Vector3[4];
    }
    void Update()
    {
        if (top[0] != new Vector3())
            Debug.DrawLine(top[0], top[1], Color.green, 1);
    }
    void OnControllerColliderHit(ControllerColliderHit col)
    {
        RaycastHit hit;
        Debug.DrawRay(col.point, col.moveDirection, Color.blue, 1);
        if (Physics.Raycast(transform.position, col.moveDirection, out hit, 0.5f))
        {
            if (hit.normal.y == transform.position.y - (GetComponent<CharacterController>().height / 2))
                return;
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
                return;

            Mesh mesh = meshCollider.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            Transform hitTransform = hit.collider.transform;
            StartCoroutine(CoroutineThing(mesh, hitTransform, vertices, triangles, hit.triangleIndex));
        }
    }
    IEnumerator CoroutineThing(Mesh mesh, Transform hitTransform, Vector3[] vertices, int[] triangles, int triangleIndex)
    {
        #region Get hit triangle
        Vector3 p0 = vertices[triangles[triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[triangleIndex * 3 + 2]];
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);
        #endregion
        Debug.DrawLine(p0, p1, Color.blue, 4);
        Debug.DrawLine(p1, p2, Color.blue, 4);
        Debug.DrawLine(p2, p0, Color.blue, 4);
        Vector3 p3 = new Vector3();
        for (int i = 10; i > -10; i--)
        {
            Vector3 p4 = new Vector3();
            if (vertices[triangles[triangleIndex * 3 + i]] == null)
                continue;
            try
            {
                p4 = hitTransform.TransformPoint(vertices[triangles[triangleIndex * 3 + i]]);
            }
            catch (System.Exception ex)
            {
                ex.Source = "ignore";
                continue;
            }
            if (p4 != p0 && p4 != p1 && p4 != p2)
            {
                int ex = 0, wai = 0, zed = 0;
                if (Mathf.FloorToInt(p4.x) == Mathf.FloorToInt(p0.x)) { ex++; }
                if (Mathf.FloorToInt(p4.y) == Mathf.FloorToInt(p0.y)) { wai++; }
                if (Mathf.FloorToInt(p4.z) == Mathf.FloorToInt(p0.z)) { zed++; }
                if (Mathf.FloorToInt(p4.x) == Mathf.FloorToInt(p1.x)) { ex++; }
                if (Mathf.FloorToInt(p4.y) == Mathf.FloorToInt(p1.y)) { wai++; }
                if (Mathf.FloorToInt(p4.z) == Mathf.FloorToInt(p1.z)) { zed++; }
                if (Mathf.FloorToInt(p4.x) == Mathf.FloorToInt(p2.x)) { ex++; }
                if (Mathf.FloorToInt(p4.y) == Mathf.FloorToInt(p2.y)) { wai++; }
                if (Mathf.FloorToInt(p4.z) == Mathf.FloorToInt(p2.z)) { zed++; }

                if (ex + wai + zed == 5)
                {
                    if (wai == 1)
                    {
                        p3 = p4;
                        break;
                    }
                }
            }
        }

        Debug.DrawLine(p0, p3, Color.red, 4);
        Debug.DrawLine(p1, p3, Color.red, 4);
        Debug.DrawLine(p2, p3, Color.red, 4);
        if (p3 != new Vector3())
        {
            quad = new Vector3[4];
            quad[3] = p3;
        }
        quad[0] = p0;
        quad[1] = p1;
        quad[2] = p2;
        StartCoroutine(CheckMoves());
        yield return null;
    }
    IEnumerator CheckMoves()
    {
        top = new Vector3[2];
        foreach (Vector3 v in quad)
        {
            if (v == new Vector3())
                continue;
            if (Mathf.FloorToInt(v.y) == Mathf.FloorToInt(top[0].y) && v.y > top[1].y)
            {
                top[1] = top[0];
                top[0] = v;
            }
            else if (v.y > top[0].y)
            {
                top[1] = top[0];
                top[0] = v;
            }
        }
        if (Vector3.Distance(transform.position, (top[0] + top[1]) / 2) <= reach)
        {  //This code block doesn't always work!
            if (!Physics.CheckCapsule((top[0] + top[1]) / 2, (top[0] + top[1]) / 2 + Vector3.up * 2, 0.5f) && !Physics.Raycast((top[0] + top[1]) / 2 + Vector3.up * 2, Vector3.down, 1.95f))
            {
                if (Physics.Raycast((top[0] + top[1]) / 2 + Vector3.up * 2 + transform.forward / 2, Vector3.down, 2.1f))
                {
                    transform.position = (top[0] + top[1]) / 2 + Vector3.up * 2;
                }
            }
        }
        yield return null;
    }
}
