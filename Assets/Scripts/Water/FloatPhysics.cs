using System;
using UnityEngine;

public class FloatPhysics : MonoBehaviour
{
    [SerializeField] public MeshFilter waterMesh;
    private Rigidbody rigidBody;
    public float multiplier = 3f;
    public float waterDragForceMultiplier = 1f;

    private void Start() 
    {
    }

    private void Awake() 
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private float getWaveHeight(float x, float z) 
    {
        Func<Vector3, Vector3, float> distance = (lhs, rhs) => {
            float xDiff = rhs.x - lhs.x;
            float zDiff = rhs.z - lhs.z;
            return xDiff * xDiff + zDiff + zDiff;
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
    public void FixedUpdate()
    {
        float waveHeight = getWaveHeight(transform.position.x, transform.position.z);
        Vector3 dragForce = Vector3.zero;
        if (transform.position.y <= waveHeight) {
            float percentSubmerged = (waveHeight - transform.position.y) / waveHeight;
            rigidBody.AddRelativeForce(new Vector3(0f, multiplier * percentSubmerged, 0f), ForceMode.Impulse);
            dragForce = rigidBody.velocity * -1 * waterDragForceMultiplier;
        }
        rigidBody.AddForce(dragForce, ForceMode.Acceleration);
    }
}
