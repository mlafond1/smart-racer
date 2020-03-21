using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : LossOfControlState {
    private float halfDuration;
    private Vector3 originalScale;
    private Vector2 originalVelocity;
    private float increaseFactor = 0.005f;
    public JumpState(CarController controller, float duration) : base(controller, duration)
    {
        this.duration = duration;
        this.halfDuration = this.duration * 0.5f;
        this.originalScale = controller.transform.localScale; 
        this.originalVelocity = base.rb.velocity;
        nextState = controller.State;
    }

    public JumpState(CarState old, float duration) : base(old, duration)
    {
        this.duration = duration;
        nextState = old;
    }

    // TODO Ignore car collisions that are not in JumpState
    public override void Drive()
    {
        Vector3 tmp = controller.transform.localScale;
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            //At end of JumpState return to previous state
            controller.ChangeState(nextState);
            //Reset scale to original value
            controller.transform.localScale = originalScale;
        }
        else if(duration >= halfDuration)
        {
            //Increase scale for illusion of gaining height
            tmp.x += increaseFactor;
            tmp.y += increaseFactor;
            controller.transform.localScale = tmp;
        }
        else
        {
            //Decrease scale for illusion of losing height
            tmp.x -= increaseFactor;
            tmp.y -= increaseFactor;
            controller.transform.localScale = tmp;
        }
        
    }
    public override bool CanChangeState(CarState newState)
    {
        if (newState.GetType() == typeof(JumpState))
        {
            JumpState other = (JumpState)newState;
            this.duration += other.duration;
            return false;
        }
        if (duration <= 0) return true;
        this.nextState = newState;
        return false;
    }

}