using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isGrounded;
    public float jumpForce = 5;
    public float jumpCount = 0;
    public float jumpCountMax = 2;
    //Must be start with looking left
    public bool isLookingLeft = true;

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

                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpCount++;
                isGrounded = false;
            }
            else if (jumpCount < jumpCountMax)
            {

                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpCount++;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }


}
