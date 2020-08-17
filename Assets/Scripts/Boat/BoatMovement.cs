using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    private Rigidbody rb;
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForceAtPosition(transform.forward * speed * Input.GetAxisRaw("Vertical"), transform.position, ForceMode.Force);
        rb.AddRelativeTorque(new Vector3(0f, Input.GetAxisRaw("Horizontal"), 0f) * 0.01f, ForceMode.Impulse);
    }
}
