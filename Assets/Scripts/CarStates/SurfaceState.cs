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
    private Surface surface;

    public SurfaceState(CarController controller, float friction, float drag, float trailDuration, Surface surface) :
        this(controller.State, friction, drag, trailDuration, surface)
    { }

    public SurfaceState(CarState old, float friction, float drag, float trailDuration, Surface surface) : base(old)
    {
        this.friction = friction > 0 ? 1 / friction : 1;
        this.ogDrag = controller.GetComponent<Rigidbody2D>().drag;
        //controller.GetComponent<Rigidbody2D>().drag = drag;
        controller.GetComponent<Rigidbody2D>().drag = friction;
        // Debug.Log(old.GetType() + ":oldState");
        this.nextState = old;
        this.trailDuration = trailDuration;
        this.surface = surface;
    }

    public override void Accelerate()
    {
        rb.AddForce(transform.up * statistics.maxSpeed * friction);
    }
    public override void Brake()
    {
        rb.AddForce(transform.up * (-statistics.maxSpeed / 1.5f) * friction);
    }

    public override bool CanChangeState(CarState newState)
    {
        Debug.Log("Change state to: " + newState.GetType());
        System.Type newStateType = newState.GetType();
        bool change = false;
        // States that can override SurfaceState
        if (newStateType == typeof(SlowedState))
        {
            // If slower than current surface
            SlowedState slow = (SlowedState)newState;
            // if (slow.GetSlowModifier() > friction) return false;
            change = slow.GetSlowModifier() < friction;
        }
        else if (newStateType == typeof(SurfaceState))
        {
            // If slower than current surface
            SurfaceState other = (SurfaceState)newState;
            //this.trailDuration += other.trailDuration;
            // if (other.friction > friction) return false;
            change = other.friction <= friction;
        }
        else if (newStateType == typeof(BoostedState))
        {
            Debug.Log("Boosted");
            // controller.GetComponent<Rigidbody2D>().drag = ogDrag;
            // return true;
            change = true;
        }
        Debug.Log("Controller touching surface: " + controller.GetComponent<Collider2D>().IsTouching(surface.GetComponent<Collider2D>()));

        // change = controller.GetComponent<Collider2D>().IsTouching(surface.GetComponent<Collider2D>()) && change;
        // If out of surface and no trail you can change
        if (!controller.GetComponent<Collider2D>().IsTouching(surface.GetComponent<Collider2D>()) && trailDuration <= 0)
        {
            change = true;
        }
        // Else start trail
        // else
        // {
        //      OnStateExit();
        // }
        // {
        //     change = false;
        // }
        // else
        // {
        //     if (trailDuration <= 0)
        //     {
        //         change = true;
        //     }
        //     // if (trailDuration <= 0 && nextState.GetType() != typeof(SurfaceState))
        //     // {
        //     //     Debug.Log("Else state (no touching surface and trailDuration <=0)");
        //     //     controller.GetComponent<Rigidbody2D>().drag = ogDrag;
        //     //     return true;
        //     // }
        // }
        // Don't change if 
        // if (!controller.GetComponent<Collider2D>().IsTouching(surface.GetComponent<Collider2D>()))
        // {
        //     return false;
        // }
        // If trail is over && current state isn't surface state
        // if (trailDuration <= 0 && nextState.GetType() != typeof(SurfaceState))
        // {
        //     Debug.Log("Else state (no touching surface and trailDuration <=0)");
        //     controller.GetComponent<Rigidbody2D>().drag = ogDrag;
        //     return true;
        // }
        if (!change) this.nextState = newState;
        else controller.GetComponent<Rigidbody2D>().drag = ogDrag;

        return change;
    }

    public override void ClearDuration()
    {
        trailDuration = 0;
    }
    public override void OnStateEnter()
    {
        //if(trailDuration > 0){
        //     Debug.Log(trailDuration + ":trail duration = 0");
        //     controller.StartCoroutine(WaitDuration());
        // }

    }

    public void OnStateExit()
    {
        //if (trailDuration > 0) controller.StartCoroutine(WaitDuration());
        //else controller.ChangeState(nextState);
        controller.ChangeState(nextState);
    }

    private IEnumerator WaitDuration()
    {
        while (trailDuration > 0)
        {
            yield return new WaitForSeconds(trailDuration >= 0.1f ? 0.1f : trailDuration);
            trailDuration -= 0.1f;
        }
        controller.ChangeState(nextState);
    }

    public CarState GetState()
    {
        return nextState;
    }
}