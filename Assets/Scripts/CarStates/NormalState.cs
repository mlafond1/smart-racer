using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalState : CarState {

    public NormalState(CarController controller) : base(controller) {

    }

    public NormalState(CarState old) : base(old) {

    }

    public override void Steer(float horizontalAxis){
        if(horizontalAxis > 1) 
            this.horizontalAxis = 1;
        else if(horizontalAxis < -1) 
            this.horizontalAxis = -1;
        else 
            this.horizontalAxis = horizontalAxis;
    }

    public override void Drive(){
        rb.rotation -= CurrentSpeed() * horizontalAxis * statistics.torqueSpeed * Time.deltaTime;

        float newDriftPercentage = statistics.driftPercentage * PercentOfMaxSpeed();
        rb.velocity = ForwardVelocity() + (RightVelocity() * newDriftPercentage);
        rb.angularVelocity = 0.0f;
    }

}