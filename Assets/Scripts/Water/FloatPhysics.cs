using System;
using UnityEngine;

public class FloatPhysics : MonoBehaviour
{
    [SerializeField] public GameObject water = null;
    [SerializeField] public MeshFilter waterMesh = null;
    [SerializeField] public MeshFilter boatMesh = null;
    [SerializeField] public MakeWaves waveScript = null;
    
    private int layerMask;
    private Rigidbody rigidBody;
    public float multiplier = 3f;
    public float waterDragForceMultiplier = 1f;

    private void Start() 
    {
    }

    private void Awake() 
    {
        rigidBody = GetComponent<Rigidbody>();
        layerMask = LayerMask.GetMask("Water");
        boatMesh.sharedMesh.MarkDynamic();
    }

    private float getWaveHeight(float x, float z) 
    {
        Func<Vector3, Vector3, float> distance = (lhs, rhs) => {
            float xDiff = rhs.x - lhs.x;
            float zDiff = rhs.z - lhs.z;
            return xDiff * xDiff + zDiff * zDiff;
        };

        Vector3 current = new Vector3(x, 0f, z);
        Vector3[] verts = waterMesh.mesh.vertices;
        Vector3 closest = waterMesh.transform.TransformPoint(verts[0]);
        for (int i = 0; i < verts.Length; i++) {
            Vector3 candidatePoint = waterMesh.transform.TransformPoint(verts[i]);
            if (distance(candidatePoint, current) <= distance(closest, current)) {
                   closest = candidatePoint;
            }
        }

        return closest.y;
    }

    int RoundUpOrDown(float value) {
        if (value % 0.5f == 0) {
            return (int) Mathf.Ceil(value);
        } 
        return (int) Mathf.Floor(value);
    }

    // Update is called once per frame
    public void Update()
    {
        Vector3 dragForce = Vector3.zero;
        Vector3[] verts = boatMesh.sharedMesh.vertices;
        for (int i = 0; i < verts.Length; i++) {
            Vector3 globalVert = boatMesh.transform.TransformPoint(verts[i]);
            float waveHeight = getWaveHeight(globalVert.x, globalVert.z);
            if (globalVert.y < waveHeight) {
                float percentSubmerged = (float)waveHeight - (float)globalVert.y;
                rigidBody.AddForceAtPosition(new Vector3(0f, percentSubmerged * multiplier, 0f), globalVert, ForceMode.Impulse);
                dragForce = -1 * rigidBody.velocity * waterDragForceMultiplier;
            }
        }
        rigidBody.AddForce(dragForce, ForceMode.Force);
    }
}
