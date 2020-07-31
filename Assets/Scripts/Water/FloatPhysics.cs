using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPhysics : MonoBehaviour
{

    public MeshFilter meshFilter; // get vertices 
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacement = 3f;

    private float getWaveHeight(float x, float z) 
    {
        Vector3[] vertices = meshFilter.mesh.vertices;
        float closest_dx = Mathf.Abs(vertices[0].x - x);
        float closest_dz = Mathf.Abs(vertices[0].z - z);
        float waveHeight = vertices[0].y;
        for (int i = 1; i < vertices.Length; i++) 
        {
            float dx = Mathf.Abs(vertices[i].x - x);
            float dz = Mathf.Abs(vertices[i].z - z);
            if (dx < closest_dx && dz < closest_dz) {
                closest_dx = dx;
                closest_dz = dz;
                waveHeight = vertices[i].y;
            }
        }
        return waveHeight;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        float waveHeight = getWaveHeight(transform.position.x, transform.position.z);
        //if (transform.position.y < waveHeight) {
        //    float multiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacement;
        //    rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * multiplier, 0f), ForceMode.Acceleration);
        //}
    }
}
