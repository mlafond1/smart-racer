using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTrackState : NormalState {
    
    public OffTrackState(CarController controller) : base(controller){
    }

    public OffTrackState(CarState old) : base(old){
    }

    public override void Accelerate(){
        rb.AddForce(transform.up * statistics.maxSpeed/3f);
    }

    public override void Brake(){
        rb.AddForce(transform.up * -statistics.maxSpeed/3f);
    }
}
