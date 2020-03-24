using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostedState : NormalState {
    
    private float duration;
    private float powerIncrease;
    private float damageIncrease;
    protected CarState nextState;

    public BoostedState(CarController controller, float duration, float powerIncrease = 0f, float damageIncrease = 0f) : base(controller){
        this.duration = duration;
        this.nextState = controller.State;
        this.powerIncrease = powerIncrease;
        this.damageIncrease = damageIncrease;
    }

    public BoostedState(CarState old, float duration, float powerIncrease = 0f, float damageIncrease = 0f) : base(old) {
        this.duration = duration;
        this.nextState = old;
        this.powerIncrease = powerIncrease;
        this.damageIncrease = damageIncrease;
    }

    public override void Accelerate(){
        rb.AddForce(transform.up * statistics.maxSpeed * 1.5f);
    }

    public override void Brake(){
        rb.AddForce(transform.up * -statistics.maxSpeed);
    }

    public override bool CanChangeState(CarState newState){
        if(newState.GetType() == typeof(BoostedState)){
            BoostedState other = (BoostedState)newState;
            this.duration = other.duration;
            controller.Statistics.UpdateOffensiveStats(-powerIncrease, -damageIncrease);
            powerIncrease = other.powerIncrease > this.powerIncrease ? other.powerIncrease : this.powerIncrease;
            damageIncrease = other.damageIncrease > this.damageIncrease ? other.damageIncrease : this.damageIncrease;
            controller.Statistics.UpdateOffensiveStats(powerIncrease, damageIncrease);
            return false;
        }
        if(newState.GetType() == typeof(LossOfControlState) || newState.GetType().IsSubclassOf(typeof(LossOfControlState))){
            return true;
        }
        if(duration <= 0) return true;
        this.nextState = newState;
        return false;
    }

    public override void OnStateEnter(){
        controller.StartCoroutine(WaitDuration());
    }

    IEnumerator WaitDuration(){
        controller.Statistics.UpdateOffensiveStats(powerIncrease, damageIncrease);
        while(duration > 0){
            yield return new WaitForSeconds(duration >= 0.1f ? 0.1f : duration);
            duration -= 0.1f;
        }
        controller.Statistics.UpdateOffensiveStats(-powerIncrease, -damageIncrease);
        controller.ChangeState(nextState);
    }
}
