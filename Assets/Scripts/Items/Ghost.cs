using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Shield {

    new public const string Name = "ghost";
    const float cooldown = 8;
    const float duration = 4;

    public override void InitialSetup(Item item){
        InitialSetup(item, duration);
    }

    public override float GetCooldown(){
        return cooldown;
    }

    protected override void ToggleOtherEffects(bool ignore){
        base.ToggleOtherEffects(ignore);
        // Effet passer au travers des autres véhicules
        CarController[] cars = GameObject.FindObjectsOfType<CarController>();
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

    private void OnTriggerEnter2D(Collider2D other){
        GuidedMissile missile;
        if(other.TryGetComponent<GuidedMissile>(out missile)){
            missile.IgnoreCar(owner, true);
        }
    }

    void OnDestroy(){
        GuidedMissile[] missiles = GameObject.FindObjectsOfType<GuidedMissile>();
        foreach(var missile in missiles){
            missile.IgnoreCar(owner, false);
        }
    }
}