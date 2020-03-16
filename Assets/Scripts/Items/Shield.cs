using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ItemEffect{

    float duration;
    bool initialized = false;

    void Update(){
        if(owner == null) return;
        if(!initialized){
            initialized = duration != 0;
        }
        else {
            duration -= Time.deltaTime;
            if(duration <= 0) Destroy(this.gameObject);
        }
        transform.position = owner.transform.position;
    }

    public void SetDuration(float duration){
        this.duration = duration;
    }

}