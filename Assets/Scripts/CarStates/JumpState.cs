using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : LossOfControlState {
    private float halfDuration;
    private Vector3 originalScale;
    private float originalDrag;
    private int originalLayer;
    private float increaseFactor = 0.05f;
    public JumpState(CarController controller, float duration) : this(controller.State, duration)
    {
    }

    public JumpState(CarState old, float duration) : base(old, duration)
    {
        this.duration = duration;
        this.halfDuration = duration * 0.5f;
        this.originalScale = controller.transform.localScale;
        this.originalDrag = rb.drag;
        this.originalLayer = controller.gameObject.layer;
        // Set drag 0 so theres no deceleration
        rb.drag = 0;
        controller.gameObject.layer++;
        // If jumping ignore not jumping layers
        Physics2D.IgnoreLayerCollision(originalLayer, controller.gameObject.layer, true);
    }

    public override void Drive()
    {
        Vector3 tmp = controller.transform.localScale;
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            //At end of JumpState return to previous state
            controller.ChangeState(nextState);
            //Reset original values
            ResetValues();
        }
        else if(duration >= halfDuration)
        {
            //Increase scale for illusion of gaining height
            tmp.x += Time.deltaTime * increaseFactor;
            tmp.y += Time.deltaTime * increaseFactor;
            controller.transform.localScale = tmp;
        }
        else
        {
            //Decrease scale for illusion of losing height
            tmp.x -= Time.deltaTime * increaseFactor;
            tmp.y -= Time.deltaTime * increaseFactor;
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

    public void ResetValues(){
        Physics2D.IgnoreLayerCollision(originalLayer, controller.gameObject.layer, false);
        controller.transform.localScale = originalScale;
        controller.gameObject.layer = originalLayer;
        rb.drag = originalDrag;
    }

}