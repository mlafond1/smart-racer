using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : LossOfControlState
{
    private float halfDuration;
    private Vector3 ogScale;
    private float ogDrag;
    private int ogLayer;
    private float sizeChangeFactor = 0.01f;
    private string jumpingLayerName = "JumpLayer";
    private int ogSpriteLayer;
    public JumpState(CarController controller, float duration) : this(controller.State, duration) { }

    public JumpState(CarState old, float duration) : base(old, duration)
    {
        this.duration = duration;
        this.halfDuration = duration * 0.5f;
        this.ogScale = controller.transform.localScale;
        this.ogDrag = rb.drag;
        this.ogLayer = controller.gameObject.layer;
        this.ogSpriteLayer = controller.GetComponent<SpriteRenderer>().sortingOrder;
        // Set drag 0 so theres no deceleration
        rb.drag = 0;
        // Set layer to jumping layer
        controller.gameObject.layer = LayerMask.NameToLayer(jumpingLayerName);
        controller.GetComponent<SpriteRenderer>().sortingOrder++;
        // If jumping ignore not jumping layers
        Physics2D.IgnoreLayerCollision(ogLayer, controller.gameObject.layer, true);
    }

    public override void Drive() { }
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
        // ** Debug.Log("Duration" + duration);
        controller.StartCoroutine(this.WaitDuration());
    }

    IEnumerator WaitDuration()
    {
        // ** float time = Time.time;
        float currentTime = 0f;
        while (currentTime < duration)
        {
            Vector3 tmp = controller.transform.localScale;
            if (currentTime <= halfDuration)
            {
                // Increase scale for illusion of gaining height
                tmp.x += sizeChangeFactor;
                tmp.y += sizeChangeFactor;
                controller.transform.localScale = Vector3.Lerp(controller.transform.localScale, tmp, currentTime);
            }
            else
            {
                // Decrease scale for illusion of losing height
                tmp.x -= sizeChangeFactor;
                tmp.y -= sizeChangeFactor;
                // Limit scale so it isn't smaller than original scale
                // If < original then = original
                controller.transform.localScale = 
                    tmp.sqrMagnitude < ogScale.sqrMagnitude ?
                        ogScale : Vector3.Lerp(controller.transform.localScale, tmp, currentTime);
            }
            currentTime += Time.deltaTime;
            yield return null;
        }
        //set duration to 0;
        duration = duration < currentTime ? 0 : duration - currentTime;
        ResetValues();
        controller.ChangeState(nextState);
        // ** time = Time.time - time;
        // ** Debug.Log("Time since start jump:" + time);
    }

    public void ResetValues()
    {
        Physics2D.IgnoreLayerCollision(ogLayer, controller.gameObject.layer, false);
        controller.transform.localScale = ogScale;
        controller.gameObject.layer = ogLayer;
        rb.drag = ogDrag;
        controller.GetComponent<SpriteRenderer>().sortingOrder = ogSpriteLayer;
    }

}