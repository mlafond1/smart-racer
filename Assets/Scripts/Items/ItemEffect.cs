using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : MonoBehaviour{

    protected CarController owner;
    protected bool initialized = false;

    public abstract void InitialSetup(Item item);

    public void SetOwner(CarController Owner){
        this.owner = Owner;
        Collider2D carCollider = Owner.gameObject.GetComponent<Collider2D>();
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        if(collider != null){
            Physics2D.IgnoreCollision(collider, carCollider);
        }
    }

    public bool SameOwner(ItemEffect other){
        return this.owner.Equals(other?.owner);
    }

    public CarController GetOwner(){
        return this.owner;
    }

    public virtual void OnReflect(CarController other){
        
    }
    

}