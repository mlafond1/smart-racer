using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarState {

    protected CarStatistics statistics;

    protected CarController controller;
    protected Transform transform;
    protected Rigidbody2D rb;

    protected float horizontalAxis = 0f;

    public CarState(CarController controller){
        this.controller = controller;
        this.statistics = controller.Statistics;
        this.rb = controller.gameObject.GetComponent<Rigidbody2D>();;
        this.transform = rb.gameObject.transform;
    }

    public CarState(CarState old){
        this.controller = old.controller;
        this.statistics = old.controller.Statistics;
        this.rb = old.rb;
        this.transform = old.rb.gameObject.transform;
    }

    public abstract void Accelerate();

    public abstract void Brake();

    public abstract void Steer(float horizontalAxis);

    public abstract void Drive();

    protected Vector2 ForwardVelocity(){
        return transform.up * Vector2.Dot(rb.velocity, transform.up);
    }

    protected Vector2 RightVelocity(){
        return transform.right * Vector2.Dot(rb.velocity, transform.right);
    }

    public float CurrentSpeed(){
        return Vector2.Dot(rb.velocity, transform.up);
    }

    public float PercentOfMaxSpeed(){
        float percent = rb.velocity.magnitude/statistics.maxSpeed;
        return percent > 1 ? 1 : percent;
    }

    public virtual bool CanChangeState(CarState newState){
        return true;
    }

    public virtual void ClearDuration(){
        
    }

}