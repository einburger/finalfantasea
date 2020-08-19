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
    public bool grounded = true;

    private void Awake() {
        rb = GetComponent<Rigidbody>();    
    }

    private float getWaveHeight(Vector3 p) 
    {
        Vector3 current = p;
        Vector3[] verts = waterMesh.mesh.vertices;
        Vector3 closest = waterMesh.transform.TransformPoint(verts[0]);
        for (int i = 0; i < verts.Length; i++) {
            Vector3 candidatePoint = waterMesh.transform.TransformPoint(verts[i]);
            if (Vector3.Distance(candidatePoint, current) <= Vector3.Distance(closest, current)) {
                   closest = candidatePoint;
            }
        }

        Debug.DrawLine(p, closest, Color.red, 0.01f);
        return closest.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dragForce = Vector3.zero;
        Vector3[] verts = floatyMesh.sharedMesh.vertices;
        int aboveWaterCount = 0;
        for (int i = 0; i < verts.Length; i++) {
            Vector3 globalVert = floatyMesh.transform.TransformPoint(verts[i]);
            float waveHeight = getWaveHeight(globalVert);
            if (globalVert.y < (waveHeight + boatDepthOffset)) {
                float percentSubmerged = ((float)waveHeight + boatDepthOffset) - (float)globalVert.y;
                rb.AddForceAtPosition(new Vector3(0f, percentSubmerged * buoyancyMultiplier, 0f), globalVert, ForceMode.Impulse);
                Debug.DrawLine(globalVert, globalVert + Vector3.up * percentSubmerged * buoyancyMultiplier, Color.green, 0.01f);
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
