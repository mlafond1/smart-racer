using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMine : Mine {

    new public const string Name = "slow-mine";
    const float cooldown = 7;
    const float duration = 2;

    float slowPercent = .4f;

    public override void InitialSetup(Item item){
        owner = item.Owner;
        originalUp = owner.transform.up;
        transform.up = originalUp;
        initialPosition = owner.transform.position - (2 * transform.up.normalized);
        initialized = true;
    }

    public override float GetCooldown(){
        return cooldown;
    }

    protected override void OnTriggerEnter2D(Collider2D otherCollider){
        ItemEffect otherEffect = otherCollider.gameObject.GetComponent<ItemEffect>();
        CarController car = otherCollider.gameObject.GetComponent<CarController>();
        if(otherEffect != null){
            if(otherEffect.GetType() == typeof(Missile)) Destroy(otherEffect.gameObject);
            Destroy(this.gameObject);
            return;
        }
        Vector2 explosionDirection = (car.transform.position - transform.position).normalized;
        Rigidbody2D carRigidbody = car.GetComponent<Rigidbody2D>();
        car.ChangeState(new SlowedState(car.State, duration, slowPercent));
        carRigidbody.velocity *= slowPercent;
        Destroy(this.gameObject);
    }

}