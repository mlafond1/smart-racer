using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleBoost : Shield {

    new public const string Name = "invincible-boost";
    const float cooldown = 12;
    const float duration = 4;
    const float powerIncrease = 10;
    const float damageIncrease = 10;

    public override void InitialSetup(Item item){
        InitialSetup(item, duration);
        owner.ChangeState(new BoostedState(owner.State, duration, powerIncrease, damageIncrease));
    }

    public override float GetCooldown(){
        return cooldown;
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
            StartCoroutine(IgnoreMultiHit(collision.collider));
        }
    }

    private IEnumerator IgnoreMultiHit(Collider2D collider){
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, true);
        yield return new WaitForSeconds(.2f);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, false);
    }
}