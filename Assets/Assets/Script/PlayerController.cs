using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float maxSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;
    public float rotationSmoothTime = 0.1f;

    public CharacterController controller;
    public Transform cameraTransform;

    public Animator animator;

    private float currentVelocity;
    private float verticalVelocity;
    public float targetAngle;

    void Start()
    {
        maxSpeed = speed;
    }

    void Update()
    {
        HandleMovement();
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        float currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
        float normalizedSpeed = currentSpeed / maxSpeed;
        animator.SetFloat("Speed", normalizedSpeed);
        animator.SetBool("isGrounded", controller.isGrounded);
    }

    void HandleMovement()
    {
        float zMovement = Input.GetAxis("Vertical");
        float xMovement = Input.GetAxis("Horizontal");

        Vector3 inputDir = new Vector3(xMovement, 0f, zMovement).normalized;
        Vector3 cameraMovement = Vector3.zero;

        if (inputDir.magnitude > 0f)
        {
            // Calculate target angle relative to camera
            targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // Smoothly rotate player to face movement direction
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Sprint support
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed * 2f : speed;

            // Move in the camera-relative direction, not the raw input direction
            cameraMovement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * currentSpeed * Time.deltaTime;
        }

        // Gravity / jump handling (accumulates properly instead of resetting every frame)
        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f; // small downward force to keep grounded check reliable

            if (Input.GetKey(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                animator.SetTrigger("Jump");
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        cameraMovement.y = verticalVelocity * Time.deltaTime;

        controller.Move(cameraMovement);
    }
}