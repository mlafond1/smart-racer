using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField]
    float maxSpeed = 30f;
    [SerializeField]
    float torqueSpeed = 6f;
    [SerializeField]
    float driftPourcentage = 0.95f;

    public float horizontalAxis = 0f; // TODO remettre private
    float currentMaxSpeed = 0f;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetOnTrack();
    }

    public void Accelerate(){
        if(!this.enabled) return;
        rb.AddForce(transform.up * currentMaxSpeed);
    }

    public void Brake(){
        if(!this.enabled) return;
        rb.AddForce(transform.up * -currentMaxSpeed/1.5f);
    }

    public void Steer(float horizontalAxis){
        if(horizontalAxis > 1) 
            this.horizontalAxis = 1;
        else if(horizontalAxis < -1) 
            this.horizontalAxis = -1;
        else 
            this.horizontalAxis = horizontalAxis;
    }

    void FixedUpdate()
    {
        float percentOfMaxSpeed = PercentOfMaxSpeed();

        bool isMovingForward = CurrentSpeed() > 0;
        //rb.AddTorque( (isMovingForward? -1 : 1) * horizontalAxis * torqueSpeed * Time.deltaTime * percentOfMaxSpeed);
        rb.rotation -= CurrentSpeed() * horizontalAxis * torqueSpeed * Time.deltaTime;

        float newDriftPourcentage = driftPourcentage * percentOfMaxSpeed;
        rb.velocity = ForwardVelocity() + (RightVelocity() * newDriftPourcentage);
        rb.angularVelocity = 0.0f;
    }

    Vector2 ForwardVelocity(){
        return transform.up * Vector2.Dot(rb.velocity, transform.up);
    }

    Vector2 RightVelocity(){
        return transform.right * Vector2.Dot(rb.velocity, transform.right);
    }

    public float CurrentSpeed(){
        return Vector2.Dot(rb.velocity, transform.up);
    }

    public float PercentOfMaxSpeed(){
        return rb.velocity.magnitude/maxSpeed;
    }

    public void SetOnTrack(){
        currentMaxSpeed = maxSpeed;
    }

    public void SetOffTrack(){
        currentMaxSpeed = maxSpeed/3f;
    }

}
