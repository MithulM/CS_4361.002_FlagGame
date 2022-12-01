using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    private float moveSpeed = 0;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float jumpHeight = 5;

    private Vector3 moveDirection;
    private Vector3 velocity;

    private float gravity = -9.8f;
    private bool isGrounded;
    private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    private Animator anim;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(moveX, moveDirection.y, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if (moveDirection != Vector3.zero)
        {
            Walk();
        }
        else if (moveDirection == Vector3.zero)
        {
            Idle();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        moveDirection *= moveSpeed;
        controller.Move(moveDirection * Time.deltaTime);


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void Idle()
    {
        anim.SetFloat("Speed", 0);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5f);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
}
