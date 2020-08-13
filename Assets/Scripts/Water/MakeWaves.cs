using UnityEngine;

public class MakeWaves : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;
    public float lastTimeStep = 0f;

    private float originalHeight = 0f;

    private int offset = 0;

    // Start is called before the first frame update
    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        Vector3 worldPoint = meshFilter.transform.TransformPoint(meshFilter.mesh.vertices[0]);
        originalHeight = worldPoint.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var verts = meshFilter.mesh.vertices;
        float scale = 0.01f;

        lastTimeStep = Time.time;
        for (int i = 0; i < verts.Length; i++) {
            //Vector3 vert = meshFilter.transform.TransformPoint(verts[i]);
            float newHeight = Mathf.PerlinNoise((i + offset) * scale, (i + offset) * scale) * 2.0f;
            verts[i] = new Vector3(verts[i].x, newHeight , verts[i].z);
        }

        meshFilter.mesh.vertices = verts;
        meshFilter.mesh.RecalculateNormals();

        offset++;
    }
}
