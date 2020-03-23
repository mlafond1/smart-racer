using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Shield {

    public override void InitialSetup(Item item){
        GhostItem ghostItem = (GhostItem)item;
        InitialSetup(ghostItem, ghostItem.Duration);
    }

    protected override void ToggleOtherEffects(bool ignore){
        base.ToggleOtherEffects(ignore);
        // Effet passer au travers des autres véhicules
        CarController[] cars = Resources.FindObjectsOfTypeAll<CarController>();
        foreach(var car in cars){
            if(!car.Equals(owner)){
                Physics2D.IgnoreCollision(ownerCollider, car.GetComponent<Collider2D>(), ignore);
            }
        }
        // TODO obstacles traversables?
    }

    protected override void OnCollisionEnter2D(Collision2D collision){
        // N'arrive pas car en mode trigger
    }
}