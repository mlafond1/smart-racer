using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    float moveSpeed = 25f;

    float torqueSpeed = -200f;

    float driftPourcentage = 0.95f;

    float horizontalAxis = 0f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Accelerate(){
        rb.AddForce(transform.up * moveSpeed);
    }

    public void Brake(){
        rb.AddForce(transform.up * -0.5f * moveSpeed);
    }

    public void SetAxis(float horizontalAxis){
        this.horizontalAxis = horizontalAxis;
    }

    void FixedUpdate()
    {
        rb.velocity = ForwardVelocity() + RightVelocity() * driftPourcentage;
        rb.angularVelocity = horizontalAxis * torqueSpeed;
    }

    Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot(rb.velocity, transform.up);
    }

    Vector2 RightVelocity()
    {
        return transform.right * Vector2.Dot(rb.velocity, transform.right);
    }
}
