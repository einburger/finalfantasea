using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPhysics : MonoBehaviour
{
    [SerializeField]
    public Transform waterTransform; // get vertices 
    private Rigidbody rigidBody;
    public float multiplier = 3f;
    public float depthOffset = 1f;

    private void Start() 
    {
            //UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
    }

    private void Awake() 
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private float getWaveHeight(float x, float z) 
    {
        return waterTransform.position.y + ((Mathf.PerlinNoise(transform.position.x + Time.time, transform.position.z + Time.time) - 0.5f) / 10f);
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        float waveHeight = getWaveHeight(transform.position.x, transform.position.z);
        Vector3 dragForce = Vector3.zero;
        if (transform.position.y <= waveHeight) {
            float depthSubmerged = (waveHeight - transform.position.y) / waveHeight;
            rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * multiplier * depthSubmerged, 0f), ForceMode.Acceleration);
            dragForce = rigidBody.velocity * -1;
        }
        rigidBody.AddForce(dragForce, ForceMode.Acceleration);
    }
}
