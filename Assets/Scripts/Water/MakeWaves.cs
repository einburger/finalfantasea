using UnityEngine;

public class MakeWaves : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;

    // Start is called before the first frame update
    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var verts = meshFilter.mesh.vertices;

        for (int i = 0; i < verts.Length; i++) {
            float newHeight = (Mathf.PerlinNoise(verts[i].x + Time.time, verts[i].z + Time.time) - 0.5f) / 10f;
            verts[i] = new Vector3(verts[i].x, newHeight, verts[i].z);
        }

        meshFilter.mesh.vertices = verts;
        meshFilter.mesh.RecalculateNormals();
    }
}
