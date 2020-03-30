using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : LossOfControlState {
    private float halfDuration;
    private Vector3 originalScale;
    private float originalDrag;
    private int originalLayer;
    private float sizeChangeFactor = 0.1f;
    private string jumpingLayerName = "JumpLayer";
    public JumpState(CarController controller, float duration) : this(controller.State, duration){}

    public JumpState(CarState old, float duration) : base(old, duration)
    {
        this.duration = duration;
        this.halfDuration = duration * 0.5f;
        this.originalScale = controller.transform.localScale;
        this.originalDrag = rb.drag;
        this.originalLayer = controller.gameObject.layer;
        // Set drag 0 so theres no deceleration
        rb.drag = 0;
        // Set layer to jumping layer
        controller.gameObject.layer = LayerMask.NameToLayer(jumpingLayerName);
        // If jumping ignore not jumping layers
        Physics2D.IgnoreLayerCollision(originalLayer, controller.gameObject.layer, true);
    }

    public override void Drive(){}
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

    public override void OnStateEnter()
    {
        controller.StartCoroutine(this.WaitDuration());
    }

    IEnumerator WaitDuration()
    {
        while (duration > 0)
        {
            yield return new WaitForSeconds(duration >= 0.1f ? 0.1f : duration);
            Vector3 tmp = controller.transform.localScale;
            if (duration >= halfDuration)
            {
                //Increase scale for illusion of gaining height
                tmp.x += 0.1f * sizeChangeFactor;
                tmp.y += 0.1f * sizeChangeFactor;
                controller.transform.localScale = tmp;
            }
            else
            {
                //Decrease scale for illusion of losing height
                tmp.x -= 0.1f * sizeChangeFactor;
                tmp.y -= 0.1f * sizeChangeFactor;
                controller.transform.localScale = tmp;
            }
            duration -= 0.1f;
        }
        ResetValues();
        controller.ChangeState(nextState);
    }

    public void ResetValues(){
        Physics2D.IgnoreLayerCollision(originalLayer, controller.gameObject.layer, false);
        controller.transform.localScale = originalScale;
        controller.gameObject.layer = originalLayer;
        rb.drag = originalDrag;
    }

}