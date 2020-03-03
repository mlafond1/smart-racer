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

    public float horizontalAxis = 0f;
    float currentMaxSpeed = 0f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetOnTrack();
    }

    public void Accelerate(){
        rb.AddForce(transform.up * currentMaxSpeed);
    }

    public void Brake(){
        rb.AddForce(transform.up * -currentMaxSpeed/2);
    }

    public void Steer(float horizontalAxis){
        this.horizontalAxis = horizontalAxis;
    }

    void FixedUpdate()
    {
        float percentOfMaxSpeed = PercentOfMaxSpeed();

        bool isMovingForward = CurrentSpeed() > 0;
        rb.AddTorque( (isMovingForward? -1 : 1) * horizontalAxis * torqueSpeed * Time.deltaTime * percentOfMaxSpeed);
        
        float newDriftPourcentage = driftPourcentage * percentOfMaxSpeed;
        rb.velocity = ForwardVelocity() + (RightVelocity() * newDriftPourcentage);
        rb.angularVelocity = 0.0f;

        //DEBUG INFO - À ENLEVER        
        Debug.Log("Axe Steer");
        Debug.Log(horizontalAxis.ToString());
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
