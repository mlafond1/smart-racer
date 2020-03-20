using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ItemEffect{

    float duration;
    bool initialized = false;
    Collider2D ownerCollider;

    void Update(){
        if(owner == null) return;
        if(!initialized){
            initialized = duration != 0;
            ownerCollider = owner.gameObject.GetComponent<Collider2D>();
            return;
        }
        duration -= Time.deltaTime;
        if(duration <= 0){
            ToggleOtherEffects(false);
            Destroy(this.gameObject);
        }
        transform.position = owner.transform.position;
    }
    
    void FixedUpdate(){
        if(owner == null || !initialized) return;
        transform.position = owner.transform.position;
        if(duration > 0){
            ToggleOtherEffects(true);
        }
    }

    public void SetDuration(float duration){
        this.duration = duration;
    }

    void ToggleOtherEffects(bool ignore){
        ItemEffect[] effects = GameObject.FindObjectsOfType<ItemEffect>();
        foreach(var effect in effects){
            if(!effect.SameOwner(this)){
                Physics2D.IgnoreCollision(ownerCollider, effect.gameObject.GetComponent<Collider2D>(), ignore);
            }
        }
    }

}