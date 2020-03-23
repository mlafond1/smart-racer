using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMine : Mine {

    float slowPercent = .4f;
    float duration;

    public override void InitialSetup(Item item){
        SlowMineItem mineItem = (SlowMineItem)item;
        owner = mineItem.Owner;
        duration = mineItem.Duration;
        originalUp = owner.transform.up;
        transform.up = originalUp;
        initialPosition = owner.transform.position - (2 * transform.up.normalized);
        initialized = true;
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