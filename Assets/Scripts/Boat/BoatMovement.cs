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
    [SerializeField] private FloatPhysics boat = null;
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
        Vector3 forceDirection = rb.transform.up.normalized * Input.GetAxisRaw("Vertical") * sailSpeed;
        rb.AddForceAtPosition(forceDirection, engineTransform.position, ForceMode.Force);
        rb.rotation *= Quaternion.Euler(0f, 0f, -1f * Input.GetAxisRaw("Horizontal") * 0.5f);
        if (boat.grounded) {
            vfxLeft.SetInt("EmissionRate", 0);
            vfxRight.SetInt("EmissionRate", 0);
        } else {
            vfxLeft.SetInt("EmissionRate", 20);
            vfxRight.SetInt("EmissionRate", 20);
        }
    }
}
