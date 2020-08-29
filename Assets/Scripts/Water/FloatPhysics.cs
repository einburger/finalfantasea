using System;
using UnityEngine;

public class FloatPhysics : MonoBehaviour
{
    [SerializeField] MeshFilter waterMesh = null;
    [SerializeField] MeshFilter floatyMesh = null;
    [SerializeField] WaveData waveData = null;

    Rigidbody rb = null;
    
    public float buoyancyMultiplier = 3f;
    public float waterDragForceMultiplier = 1f;
    public float boatDepthOffset = 2f;
    public bool grounded = true;

    private void Awake() {
        rb = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dragForce = Vector3.zero;
        Vector3[] verts = floatyMesh.sharedMesh.vertices;
        int aboveWaterCount = 0;
        for (int i = 0; i < verts.Length; i++) {
            Vector3 globalVert = floatyMesh.transform.TransformPoint(verts[i]);
            float waveHeight = waveData.GetWaveHeight(globalVert);
            if (globalVert.y < (waveHeight + boatDepthOffset)) {
                float percentSubmerged = ((float)waveHeight + boatDepthOffset) - (float)globalVert.y;
                rb.AddForceAtPosition(new Vector3(0f, percentSubmerged * buoyancyMultiplier, 0f), globalVert, ForceMode.Impulse);
                dragForce = -1 * rb.velocity * waterDragForceMultiplier;
                aboveWaterCount++;
            } 
        }
        if (aboveWaterCount > 2) {
            grounded = false;
        } else {
            grounded = true;
        }
        rb.AddForce(dragForce, ForceMode.Force);
    }
}
