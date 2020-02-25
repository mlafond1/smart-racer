using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField]
    float maxSpeed = 30f;
    [SerializeField]
    float torqueSpeed = 25f;
    [SerializeField]
    float driftPourcentage = 0.75f;

    float horizontalAxis = 0f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Accelerate(){
        rb.AddForce(transform.up * maxSpeed);
    }

    public void Brake(){
        rb.AddForce(transform.up * -maxSpeed/2);
    }

    public void SetAxis(float horizontalAxis){
        this.horizontalAxis = horizontalAxis;
    }

    void FixedUpdate()
    {
        float percentOfMaxSpeed = rb.velocity.magnitude/maxSpeed;

        rb.AddTorque(-horizontalAxis * torqueSpeed * Time.deltaTime * percentOfMaxSpeed);
        
        float newDriftPourcentage = driftPourcentage * percentOfMaxSpeed;
        rb.velocity = ForwardVelocity() + (RightVelocity() * newDriftPourcentage);
        rb.angularVelocity = 0.0f;
    }

    private Vector2 ForwardVelocity(){
        return transform.up * Vector2.Dot(rb.velocity, transform.up);
    }

    private Vector2 RightVelocity(){
        return transform.right * Vector2.Dot(rb.velocity, transform.right);
    }
}
