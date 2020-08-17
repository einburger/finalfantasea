using UnityEngine;

public class WaveNoise : MonoBehaviour {
    Renderer rend;
    RenderTexture rendTex;
    Texture2D noise;
    Color[] pix;
    int width, height;

    public ComputeShader noiseKernel;
    public ComputeBuffer vertexData;
    float offset = 0.0f;
    [Range(0f, 10f)] public float steepness = 1.0f;
    [Range(0f, 1f)] public float wavelength = 1.0f;

    MeshFilter waveMeshFilter;
    Vector3[] inVerts;
    Vector3[] originalVerts;

    void copyArray(Vector3[] dest, Vector3[] src) {
        for (int i = 0; i < src.Length; i++) {
            dest[i].x = src[i].x;
            dest[i].y = src[i].y;
            dest[i].z = src[i].z;
        }
    }

    void Awake() {
        waveMeshFilter = GetComponent<MeshFilter>();
        inVerts = waveMeshFilter.sharedMesh.vertices;
        originalVerts = new Vector3[inVerts.Length];
        copyArray(originalVerts, inVerts);
        Debug.Log(inVerts.Length);
        vertexData = new ComputeBuffer(inVerts.Length, sizeof(float) * 3);
    }

    void FixedUpdate() {
        int kernelID = noiseKernel.FindKernel("CSMain");
        vertexData.SetData(originalVerts);
        noiseKernel.SetFloat("NoiseOffset", offset);
        noiseKernel.SetFloat("SteepnessMultiplier", steepness);
        noiseKernel.SetFloat("WavelengthMultiplier", wavelength);
        noiseKernel.SetBuffer(kernelID, "vertices", vertexData);
        noiseKernel.Dispatch(kernelID, inVerts.Length, 1, 1);
        vertexData.GetData(inVerts);

        waveMeshFilter.sharedMesh.vertices = inVerts;
        waveMeshFilter.sharedMesh.RecalculateBounds();
        waveMeshFilter.sharedMesh.RecalculateNormals();
        waveMeshFilter.sharedMesh.RecalculateTangents();
        offset += 0.0003f;
    }

    private void OnDestroy() {
        waveMeshFilter.sharedMesh.vertices = originalVerts;
        waveMeshFilter.sharedMesh.RecalculateBounds();
        waveMeshFilter.sharedMesh.RecalculateNormals();
        waveMeshFilter.sharedMesh.RecalculateTangents();
    }

    public float SampleHeight(int x, int z) {
        return noise.GetPixel(x, z).r;
    }
}
