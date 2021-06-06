using System.Collections.Generic;
using MeshGeneration;
using UnityEditor;
using UnityEngine;

public class TriangleTest : MonoBehaviour
{
    public int numPoints;
    
    public Transform aTransform;
    public Transform bTransform;
    public Transform cTransform;

    private List<Vector3> points = new List<Vector3>();
    
    [ContextMenu("Execute")]
    private void Execute()
    {
        if (!aTransform || !bTransform || !cTransform)
            return;
        
        points.Clear();

        for (int i = 0; i < numPoints; i++)
            points.Add(MeshUtils.RandomPointInTriangle(aTransform.position, bTransform.position, cTransform.position));
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.DrawLine(aTransform.position, cTransform.position);
        Handles.DrawLine(aTransform.position, bTransform.position);
        Handles.DrawLine(bTransform.position, cTransform.position);

        Handles.color = Color.red;
        foreach (Vector3 point in points)
            Handles.DrawWireArc(point, Vector3.back, Vector3.up, 360f, 0.03f);
    }
}