using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Animator animController;
    public CharacterController controller;
    public Transform cameraTransform;

    public float speed = 6f;
    public float turnSmoothing = 0.1f;

    private bool walking = false;

    float turnOmega;
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (!controller.isGrounded) {
            Vector3 vec = Vector3.zero;
            controller.SimpleMove(vec);
        }

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnOmega, turnSmoothing);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            if (!walking) {
                animController.SetTrigger("walkTrigger");
                walking = true;
            }
            
        } else {
            Vector3 moveDir = Vector3.forward * 0f; 

            if (walking) {
                animController.SetTrigger("idleTrigger");
                walking = false;
            }
        }
    }
}
