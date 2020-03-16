using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour{

    protected CarController owner;

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
    

}