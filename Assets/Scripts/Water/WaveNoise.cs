using System;
using UnityEngine;

public class WaveNoise : MonoBehaviour {
    Renderer rend;
    RenderTexture rendTex;
    Texture2D noise;
    Color[] pix;
    int width, height;

    public ComputeShader noiseKernel;
    public ComputeBuffer vertexData;
    public ComputeBuffer waveData;
    float offset = 0.0f;
    [SerializeField] float xOffset, yOffset;
    [Range(0f, 0.1f)] public float wavespeed = 0.0003f;

    [Serializable] public struct Wave {
        public Vector2 windDir;
        [Range(0f, 10f)] public float steepness;
        [Range(0f, 5f)] public float wavelength;
    };

    [SerializeField] Wave[] waves;
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

    void Start() {
        waveMeshFilter = GetComponent<MeshFilter>();
        inVerts = new Vector3[waveMeshFilter.sharedMesh.vertices.Length];
        originalVerts = new Vector3[inVerts.Length];
        copyArray(inVerts, waveMeshFilter.sharedMesh.vertices);
        copyArray(originalVerts, inVerts);
        vertexData = new ComputeBuffer(inVerts.Length, sizeof(float) * 3);
        waveData = new ComputeBuffer(waves.Length, sizeof(float) * 4, ComputeBufferType.Structured);
    }

    void FixedUpdate() {
        int kernelID = noiseKernel.FindKernel("CSMain");
        vertexData.SetData(originalVerts);
        waveData.SetData(waves);

        noiseKernel.SetFloat("xOffset", xOffset);
        noiseKernel.SetFloat("yOffset", yOffset);
        noiseKernel.SetFloat("NoiseOffset", offset);
        noiseKernel.SetInt("NumberOfWaves", waves.Length);

        noiseKernel.SetBuffer(kernelID, "vertices", vertexData);
        noiseKernel.SetBuffer(kernelID, "waves", waveData);

        noiseKernel.Dispatch(kernelID, inVerts.Length, 1, 1);
        vertexData.GetData(inVerts);

        waveMeshFilter.mesh.vertices = inVerts;
        waveMeshFilter.mesh.RecalculateBounds();
        waveMeshFilter.mesh.RecalculateNormals();
        waveMeshFilter.mesh.RecalculateTangents();
        offset += wavespeed;
    }
}
