using System;
using UnityEngine;

public class FloatPhysics : MonoBehaviour
{
    [SerializeField] MeshFilter waterMesh = null;
    [SerializeField] MeshFilter floatyMesh = null;

    Rigidbody rb = null;
    
    public float buoyancyMultiplier = 3f;
    public float waterDragForceMultiplier = 1f;
    public float boatDepthOffset = 2f;

    private void Awake() {
        rb = GetComponent<Rigidbody>();    
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

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dragForce = Vector3.zero;
        Vector3[] verts = floatyMesh.sharedMesh.vertices;
        for (int i = 0; i < verts.Length; i++) {
            Vector3 globalVert = floatyMesh.transform.TransformPoint(verts[i]);
            float waveHeight = getWaveHeight(globalVert.x, globalVert.z);
            if (globalVert.y < (waveHeight + boatDepthOffset)) {
                float percentSubmerged = ((float)waveHeight + boatDepthOffset) - (float)globalVert.y;
                rb.AddForceAtPosition(new Vector3(0f, percentSubmerged * buoyancyMultiplier, 0f), globalVert, ForceMode.Acceleration);
                dragForce = -1 * rb.velocity * waterDragForceMultiplier;
            }
        }
        rb.AddForce(dragForce, ForceMode.Force);
    }

    void Update() {
        Vector3 rotation = rb.rotation.eulerAngles;

        // rotation.z = Mathf.Clamp(rotation.z, -45f, 45f);
        // rb.rotation = Quaternion.Euler(0f, 0f, rotation.z);

        // rotation.x = Mathf.Clamp(rotation.x, -45f, 45f);
        // rb.rotation = Quaternion.Euler(rotation.x, 0f, 0f);
    }
}
