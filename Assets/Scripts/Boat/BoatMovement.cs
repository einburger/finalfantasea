using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float sailSpeed = 3f;
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private VisualEffect vfxLeft;
    [SerializeField] private VisualEffect vfxRight;
    [SerializeField] private Transform engineTransform = null;
    [SerializeField] private Volume postProc = null;

    int vfxLeftID;
    int vfxRightID;

    private Rigidbody rb;
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable() {
        DepthOfField dof;
        postProc.profile.TryGet(out dof);
        dof.active = true;
    }

    void OnDisable() {
        DepthOfField dof;
        postProc.profile.TryGet(out dof);
        dof.active = false;
    }

    void FixedUpdate()
    {
        // rb.AddForceAtPosition(transform.forward * speed * Input.GetAxisRaw("Vertical"), transform.position, ForceMode.Force);
        Vector3 forceDirection = rb.transform.up.normalized * Input.GetAxisRaw("Vertical") * sailSpeed;
        // Debug.DrawLine(position, engineTransform.position, Color.blue, 0.01f);
        // rb.AddRelativeForce(forceDirection, ForceMode.Force);
        rb.AddForceAtPosition(forceDirection, engineTransform.position, ForceMode.Force);
        // rb.AddRelativeTorque(new Vector3(0f, 0f, Input.GetAxisRaw("Horizontal")) * -turnSpeed, ForceMode.Force);
        rb.rotation *= Quaternion.Euler(0f, 0f, -1f * Input.GetAxisRaw("Horizontal") * 0.5f);
        float velocity = rb.velocity.sqrMagnitude;
        if (velocity > 1f) {
            velocity /= 10f;
            vfxLeft.SetInt("EmissionRate", (int)velocity);
            vfxRight.SetInt("EmissionRate", (int)velocity);
        }
    }
}
