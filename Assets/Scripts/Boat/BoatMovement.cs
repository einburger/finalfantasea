using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float sailSpeed = 3f;
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private VisualEffect vfxLeft;
    [SerializeField] private VisualEffect vfxRight;

    int vfxLeftID;
    int vfxRightID;

    private Rigidbody rb;
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // rb.AddForceAtPosition(transform.forward * speed * Input.GetAxisRaw("Vertical"), transform.position, ForceMode.Force);
        rb.AddRelativeForce(new Vector3(0f, Input.GetAxisRaw("Vertical") * sailSpeed, 0f), ForceMode.Force);
        rb.AddRelativeTorque(new Vector3(0f, 0f, Input.GetAxisRaw("Horizontal")) * -turnSpeed, ForceMode.Force);
        float velocity = rb.velocity.sqrMagnitude;
        if (velocity > 1f) {
            velocity /= 10f;
            vfxLeft.SetInt("EmissionRate", (int)velocity);
            vfxRight.SetInt("EmissionRate", (int)velocity);
        }
    }
}
