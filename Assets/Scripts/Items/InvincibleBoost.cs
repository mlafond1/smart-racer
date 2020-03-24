using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleBoost : Shield {

    public override void InitialSetup(Item item){
        InvincibleBoostItem invincibleItem = (InvincibleBoostItem)item;
        InitialSetup(invincibleItem, invincibleItem.Duration);
        owner.ChangeState(new BoostedState(owner.State, invincibleItem.Duration, invincibleItem.PowerIncrease, invincibleItem.DamageIncrease));
    }

    protected override void OnCollisionEnter2D(Collision2D collision){
        CarController otherCar;
        if(collision.collider.TryGetComponent<CarController>(out otherCar)){
            float percentOfMaxSpeed = owner.GetComponent<Rigidbody2D>().velocity.magnitude / owner.Statistics.maxSpeed;
            // Damage
            otherCar.ApplyDamage(owner.Statistics.attackDamage * percentOfMaxSpeed);
            // Bump
            Vector3 directionAway = otherCar.transform.position - transform.position;
            directionAway.z = 0;
            otherCar.ChangeState(new LossOfControlState(otherCar.State, 0.07f));
            collision.rigidbody.AddForce(directionAway.normalized * owner.Statistics.power * otherCar.Statistics.ejectionRate, ForceMode2D.Impulse);
        }
    }

}