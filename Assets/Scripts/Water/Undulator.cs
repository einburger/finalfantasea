using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Undulator : MonoBehaviour
{
    private MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3[] points = meshFilter.mesh.vertices;
        for (int i = 0; i < points.Length; i++) {
            var perl = Mathf.PerlinNoise(points[i].x, points[i].z); 
            points[i].y += Mathf.Cos(perl + 2f * Time.deltaTime) * 30f;
        }
        meshFilter.mesh.vertices = points;
        meshFilter.mesh.RecalculateNormals();
    }
}
