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
        duration -= Time.deltaTime;
        if(duration <= 0){
            controller.ChangeState(nextState);
        }
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

}