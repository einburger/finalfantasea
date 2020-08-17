using UnityEngine;

public class MakeWaves : ScriptableObject
{
    private MeshFilter meshFilter;
    private Mesh mesh;
    private float[,] noise;
    public float lastTimeStep = 0f;

    private float originalHeight = 0f;

    private float offset = 0f;

    void createNoise(int width, int height) {
        float scale = 33f;
        noise = new float[width, height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float perlinX = x / scale;
                float perlinY = y / scale;
                float value = Mathf.PerlinNoise(perlinX, perlinY) * 10f;
                noise[x, y] = value;                    
            }
        }
    }

    void updateHeights() {
        var verts = meshFilter.mesh.vertices;
        for (int y = 0; y < 33; y++) {
            for (int x = 0; x < 33; x++) {
                float newHeight = Mathf.PerlinNoise((((float)x + offset) / 33f), (((float)y + offset) / 33f)) * 2f - 1.0f; 
                verts[y * 33 + x].y = newHeight; 
            }
        }
        offset += 0.06f;
        meshFilter.mesh.vertices = verts;
        meshFilter.mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        updateHeights();
    }
}
