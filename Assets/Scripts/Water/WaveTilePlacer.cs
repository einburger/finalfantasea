using UnityEngine;

class WaveTile : ScriptableObject 
{
    GameObject waveTileObject;
    MeshFilter waveMeshFilter;

    public WaveTile(int x, int z) 
    {
        waveTileObject = GameObject.CreatePrimitive(PrimitiveType.Plane); 
        waveTileObject.transform.position = new Vector3(x, 0f, z);
        waveMeshFilter = waveTileObject.GetComponent<MeshFilter>();
    }

    public void UpdateWaves(ref WaveNoise waveNoise, int stride) {
        Vector3[] verts = waveMeshFilter.sharedMesh.vertices;

        for (int y = 0; y < 10; y++) {
            for (int x = 0; x < 10; x++) {
                // Vector3 wavePoint = waveTileObject.transform.TransformPoint(verts[y * 4 + x]);
                // int sampleX = (int)((wavePoint.x - Mathf.Floor(wavePoint.x)) * 10f) - 5;
                // int sampleY = (int)((wavePoint.y - Mathf.Floor(wavePoint.y)) * 10f) - 5;
                verts[stride + y * 10 + x].y = waveNoise.SampleHeight(x, y);
            }
        }

        waveMeshFilter.sharedMesh.vertices = verts;
        waveMeshFilter.sharedMesh.RecalculateNormals();
    }
};

public class WaveTilePlacer : MonoBehaviour
{
    public int resolution = 4;
    const int tileSize = 10;
    WaveTile[,] waveTiles;
    [SerializeField] public WaveNoise waveNoise;

    void Awake()
    {
        waveTiles = new WaveTile[resolution, resolution];
        // waveNoise = new WaveNoise(resolution * tileSize);

        for (int z = 0; z < resolution; z++) {
            for (int x = 0; x < resolution; x++) {
                waveTiles[x, z] = new WaveTile((x * tileSize) + (tileSize / 2), 
                                               (z * tileSize) + (tileSize / 2));
            }
        }
    }

    void FixedUpdate()
    {
        //waveNoise.UpdateNoise();
        for (int z = 0; z < resolution; z++) {
            for (int x = 0; x < resolution; x++) {
                waveTiles[x, z].UpdateWaves(ref waveNoise, (z+x)*tileSize*tileSize);
            }
        }    
    }
}