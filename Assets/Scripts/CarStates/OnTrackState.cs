using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrackState : NormalState {
    
    public OnTrackState(CarController controller) : base(controller){
    }

    public OnTrackState(CarState old) : base(old){
    }

    public override void Accelerate(){
        rb.AddForce(transform.up * statistics.maxSpeed);
    }

    public override void Brake(){
        rb.AddForce(transform.up * -statistics.maxSpeed/1.5f);
    }
}
