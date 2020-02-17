using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    float moveSpeed = 50f;

    float torqueSpeed = -200f;

    float driftPourcentage = 0.95f;

    void Start()
    {
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Movement
        if (Input.GetButton("Accelerate"))
        {
            rb.AddForce(transform.up * moveSpeed);
        }
        else if (Input.GetButton("Break"))
        {
            rb.AddForce(transform.up * -0.5f * moveSpeed);
        }

        rb.velocity = ForwardVelocity() + RightVelocity() * driftPourcentage;
        rb.angularVelocity = Input.GetAxis("Horizontal") * torqueSpeed;
    }

    Vector2 ForwardVelocity()
    {
        return transform.up *
        Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.up);
    }

    Vector2 RightVelocity()
    {
        return transform.right *
        Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.right);
    }
}
