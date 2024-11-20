using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AnimatorBrain
{
    public bool isGrounded;
    public float jumpForce = 7;
    public float jumpCount = 0;
    public float jumpCountMax = 2;
    //Must be start with looking left
    public bool isLookingLeft = true;
    private const int UPPERBODY = 0;
    private const int LOWERBODY = 1;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        RotatePlayer();
        Jump();
    }

    public void RotatePlayer()
    {
        if (isLookingLeft && Input.GetKeyDown(KeyCode.D))
        {
            transform.Rotate(Vector3.up, -180);
            isLookingLeft = false;
        }
        else if (!isLookingLeft && Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(Vector3.up, 180);
            isLookingLeft = true;
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded)
            {
                PerformJump();
                jumpCount++;
                isGrounded = false;
            }
            else if (jumpCount < jumpCountMax)
            {
                // Reset vertical velocity before second jump
                Vector3 velocity = rb.velocity;
                velocity.y = 0;
                rb.velocity = velocity;

                PerformJump();
                jumpCount++;
            }
        }
    }

    private void PerformJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }
}