using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : LossOfControlState
{
    private Vector3 originalScale;
    private Vector3 center;
    private float originalDrag;
    private int originalLayer;
    private float sizeChangeFactor = 0.05f;
    public FallState(CarController controller, float duration, Vector3 center) : this(controller.State, duration, center) { }

    public FallState(CarState old, float duration, Vector3 center) : base(old, duration)
    {
        this.duration = duration;
        this.originalScale = controller.transform.localScale;
        this.originalDrag = rb.drag;
        this.originalLayer = controller.gameObject.layer;
        this.center = center;
        // Set drag 0 so theres no deceleration
        rb.drag = 0;
        controller.gameObject.layer++;
        // If jumping ignore not jumping layers
        Physics2D.IgnoreLayerCollision(originalLayer, controller.gameObject.layer, true);
    }

    public override void Drive() { }
    public override bool CanChangeState(CarState newState)
    {
        if (newState.GetType() == typeof(FallState))
        {
            FallState other = (FallState)newState;
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
        Vector3 tmp = rb.velocity;
        tmp.x = center.x - controller.transform.position.x;
        tmp.y = center.y - controller.transform.position.y;
        controller.transform.up = tmp;
        tmp.x = 0;
        tmp.y = 0;
        rb.velocity = tmp;

        while (duration > 0)
        {
            yield return new WaitForSeconds(duration >= 0.1f ? 0.1f : duration);
            // Move towards center of the fall
            controller.transform.position = Vector3.MoveTowards(controller.transform.position, center, sizeChangeFactor);
            tmp = controller.transform.localScale;

            //Decrease scale for illusion of losing height
            tmp.x -= 0.1f * sizeChangeFactor;
            tmp.y -= 0.1f * sizeChangeFactor;

            controller.transform.localScale = (tmp.x > 0 || tmp.y > 0) ? tmp : controller.transform.localScale;
            duration -= 0.1f;
        }
        ResetValues();
        controller.Respawn();
        controller.ChangeState(nextState);
    }

    public void ResetValues()
    {
        Physics2D.IgnoreLayerCollision(originalLayer, controller.gameObject.layer, false);
        controller.transform.localScale = originalScale;
        controller.gameObject.layer = originalLayer;
        rb.drag = originalDrag;
    }

}