using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LossOfControlState : CarState {

    protected float duration;
    protected CarState nextState;

    public LossOfControlState(CarController controller, float duration) : base(controller){
        this.duration = duration;
        nextState = controller.State;
    }

    public LossOfControlState(CarState old, float duration) : base(old) {
        this.duration = duration;
        nextState = old;
    }

    public override void Accelerate(){
        
    }

    public override void Brake(){
        
    }

    public override void Steer(float horizontalAxis){
        
    }

    public override void Drive(){
        rb.angularVelocity *= 0.5f;
    }

    public override void ClearDuration(){
        duration = 0;
    }

    public override bool CanChangeState(CarState newState){
        if(newState.GetType() == typeof(LossOfControlState)){
            LossOfControlState other = (LossOfControlState)newState;
            this.duration += other.duration;
            return false;
        }
        if(duration <= 0) return true;
        this.nextState = newState;
        return false;
    }

    public override void OnStateEnter(){
        controller.StartCoroutine(WaitDuration());
    }

    IEnumerator WaitDuration(){
        while(duration > 0){
            yield return new WaitForSeconds(duration >= 0.1f ? 0.1f : duration);
            duration -= 0.1f;
        }
        controller.ChangeState(nextState);
    }

}