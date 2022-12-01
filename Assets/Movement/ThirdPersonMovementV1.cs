using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovementV1 : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float speed;
    private float turnSmoothVelocity;
    private float gravity = 9.81f;
    private Animator anim;
    private Vector3 movement;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(controller.isGrounded)
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized * speed;
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                anim.SetFloat("Speed", 1f);
            }
            else
            {
                anim.SetFloat("Speed", 0f);
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                movement.y = 10;
            }
        }
        else
        {
            movement.y -= gravity * Time.deltaTime;
        }
        controller.Move(movement.normalized * speed * Time.deltaTime);
    }
}
