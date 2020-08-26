using UnityEngine;

public class WaveData : MonoBehaviour
{
    MeshFilter waterMesh = null;

    void Start() {
        waterMesh = GetComponent<MeshFilter>();
    }

    public float GetWaveHeight(Vector3 p) 
    {
        Vector3 current = p;
        Vector3[] verts = waterMesh.mesh.vertices;
        Vector3 closest = waterMesh.transform.TransformPoint(verts[0]);
        for (int i = 0; i < verts.Length; i++) {
            Vector3 candidatePoint = waterMesh.transform.TransformPoint(verts[i]);
            if (Vector3.Distance(candidatePoint, current) <= Vector3.Distance(closest, current)) {
                   closest = candidatePoint;
            }
        }

        Debug.DrawLine(p, closest, Color.red, 0.01f);
        return closest.y;
    }
}
