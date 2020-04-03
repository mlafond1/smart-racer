using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowedState : NormalState {
    
    private float duration;
    private float slowModifier;
    protected CarState nextState;

    public SlowedState(CarController controller, float duration, float slowPercent) : base(controller){
        this.duration = duration;
        this.nextState = controller.State;
        this.slowModifier = 1 - slowPercent;
        controller.StartCoroutine(WaitDuration());
    }

    public SlowedState(CarState old, float duration, float slowPercent) : base(old) {
        this.duration = duration;
        this.nextState = old;
        this.slowModifier = 1 - slowPercent;
        controller.StartCoroutine(WaitDuration());
    }

    public override void Accelerate(){
        rb.AddForce(transform.up * statistics.maxSpeed * slowModifier);
    }

    public override void Brake(){
        rb.AddForce(transform.up * (-statistics.maxSpeed/1.5f) * slowModifier);
    }

    public override void ClearDuration(){
        duration = 0;
    }

    public override bool CanChangeState(CarState newState){
        System.Type newStateType = newState.GetType();
        if(newStateType == typeof(SlowedState)){
            SlowedState other = (SlowedState)newState;
            this.duration = other.duration;
            return true;
        }
        if(newStateType == typeof(BoostedState)){
            ClearDuration();
        }
        if(newStateType == typeof(LossOfControlState) || newStateType.IsSubclassOf(typeof(LossOfControlState))){
            return true;
        }
        if(duration <= 0) return true;
        this.nextState = newState;
        return false;
    }

    IEnumerator WaitDuration(){
        while(duration > 0){
            yield return new WaitForSeconds(duration >= 0.1f ? 0.1f : duration);
            duration -= 0.1f;
        }
        controller.ChangeState(nextState);
    }

    public float GetSlowModifier(){ return slowModifier;}
}
