using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceState : NormalState
{

    private float friction;
    // protected float drag;
    private CarState nextState;
    private float ogDrag;
    private float trailDuration;

    private float newDrift;
    private Surface surface;

    public SurfaceState(CarController controller, float friction, float drag, float trailDuration, Surface surface) :
        this(controller.State, friction, drag, trailDuration, surface)
    { }

    public SurfaceState(CarState old, float friction, float drag, float trailDuration, Surface surface) : base(old)
    {
        this.friction = friction > 0 ? (1 / friction) : 1;
        this.newDrift = drag > 1 ? (1 / drag) : 1;
        this.ogDrag = controller.GetComponent<Rigidbody2D>().drag;
        controller.GetComponent<Rigidbody2D>().drag = drag;
        this.nextState = old;
        this.trailDuration = trailDuration;
        this.surface = surface;
    }

    public override void Accelerate()
    {
        Vector3 newForce = transform.up * (statistics.maxSpeed * friction);
        rb.AddForce(newForce.sqrMagnitude <= transform.up.sqrMagnitude ? transform.up : newForce);
    }
    public override void Brake()
    {
        Vector3 newForce = transform.up * ((-statistics.maxSpeed / 1.5f) * friction);
        rb.AddForce(newForce.sqrMagnitude <= transform.up.sqrMagnitude ? transform.up : newForce);
    }

    public override void Drive()
    {
        rb.rotation -= CurrentSpeed() * horizontalAxis * statistics.torqueSpeed * newDrift * Time.deltaTime;

        float newDriftPercentage = newDrift; // * PercentOfMaxSpeed();
        rb.velocity = ForwardVelocity() + (RightVelocity() * newDriftPercentage);
        //rb.angularVelocity = 0.0f;
    }

    public override bool CanChangeState(CarState newState)
    {
        System.Type newStateType = newState.GetType();
        bool change = false;
        // States that can override SurfaceState
        if (newStateType == typeof(SlowedState))
        {
            // If slower than current surface
            SlowedState slow = (SlowedState)newState;
            change = slow.GetSlowModifier() < friction;
        }
        else if (newStateType == typeof(SurfaceState))
        {
            // If slower than current surface
            SurfaceState other = (SurfaceState)newState;
            change = other.friction <= friction;
        }
        else if (newStateType == typeof(BoostedState))
        {
            change = true;
        }

        // If out of surface and no trail you can change
        if (!controller.GetComponent<Collider2D>().IsTouching(surface.GetComponent<Collider2D>()) && trailDuration <= 0)
        {
            change = true;
        }
        if (!change) this.nextState = newState;
        else ResetValues();

        return change;
    }

    public override void ClearDuration()
    {
        trailDuration = 0;
    }
    public override void OnStateEnter() { }

    public void OnStateExit()
    {
        if (trailDuration > 0)
        {   
            controller.GetComponent<TrailRenderer>().time = trailDuration;
            controller.GetComponent<TrailRenderer>().enabled = true;
            controller.StartCoroutine(TrailRender());
            controller.StartCoroutine(WaitDuration());
        }
        else controller.ChangeState(nextState);
    }

    private IEnumerator TrailRender(){
        float currentTime = 0;
        controller.GetComponent<TrailRenderer>().endColor = Color.black;
        controller.GetComponent<TrailRenderer>().startColor = Color.black;
        while (currentTime < trailDuration)
        {
            controller.GetComponent<TrailRenderer>().endColor = Color.Lerp(Color.black, Color.clear, currentTime);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator WaitDuration()
    {
        float currentTime = 0;
        while (currentTime < trailDuration)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        trailDuration = 0f;
        controller.ChangeState(nextState);
    }

    public CarState GetState()
    {
        return nextState;
    }

    public void ResetValues()
    {
        controller.GetComponent<Rigidbody2D>().drag = ogDrag;
        controller.GetComponent<TrailRenderer>().enabled = false;
        controller.GetComponent<TrailRenderer>().time = 0;
    }
}