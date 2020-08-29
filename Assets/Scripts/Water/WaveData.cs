using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveData : MonoBehaviour
{
    MeshFilter waterMesh = null;
    Dictionary<Tuple<int, int>, List<int>> buckets = new Dictionary<Tuple<int, int>, List<int>>(); 
    const int BucketSize = 5;

    public Tuple<int, int> Hash2DCoord(Vector3 p) { 
        return new Tuple<int, int>((int)(p.x / (float)BucketSize), 
                                   (int)(p.z / (float)BucketSize)); 
    }

    void FillBuckets() {
        waterMesh = GetComponent<MeshFilter>();
        Vector3[] verts = waterMesh.mesh.vertices;
        for (int i = 0; i < verts.Length; i++) {
            Vector3 point = waterMesh.transform.TransformPoint(verts[i]);
            var hash = Hash2DCoord(point);
            try {
                buckets[hash].Add(i);
            } catch (KeyNotFoundException) {
                buckets.Add(hash, new List<int>(i));
            }
        }
    } 

    void Start() {
        FillBuckets();
    }

    public float GetWaveHeight(Vector3 p) 
    {
        Vector3 current = p;
        List<int> indices = buckets[Hash2DCoord(p)];
        // foreach (int i in indices)  {
        //     Vector3 v = waterMesh.transform.TransformPoint(waterMesh.mesh.vertices[i]);
        //     Debug.DrawLine(v, v + 2f * Vector3.up, Color.red, 0.01f);
        // }
        Vector3[] verts = waterMesh.mesh.vertices; 
        Vector3 closest = waterMesh.transform.TransformPoint(verts[indices[0]]);
        for (int i = 0; i < indices.Count; i++) {
            Vector3 candidatePoint = waterMesh.transform.TransformPoint(verts[indices[i]]);
            if (Vector3.Distance(candidatePoint, current) <= Vector3.Distance(closest, current)) {
                   closest = candidatePoint;
            }
        }
        return closest.y;
    }
}
