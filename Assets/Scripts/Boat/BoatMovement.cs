using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    private float turnOmega = 0.1f;
    private float turnSmoothing = 0.1f;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform engine;
    [SerializeField] private float speed = 2f;

    void FixedUpdate()
    {
        rigidbody.AddRelativeForce(new Vector3(0f, 0f, Input.GetAxisRaw("Vertical") * speed), ForceMode.Force);
        rigidbody.AddRelativeTorque(new Vector3(0f, Input.GetAxisRaw("Horizontal") * speed, 0f), ForceMode.Force);
    }
}
