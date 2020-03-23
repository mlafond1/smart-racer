using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : ItemEffect {

    protected Vector3 initialPosition;
    protected Vector3 originalUp;

    float power = 20;
    float damage = 12;
    float lossOfControlTime = 0.3f;

    public override void InitialSetup(Item item){
        MineItem mineItem = (MineItem)item;
        owner = mineItem.Owner;
        originalUp = owner.transform.up;
        transform.up = originalUp;
        initialPosition = owner.transform.position - (2 * transform.up.normalized);
        initialized = true;
    }  

    void FixedUpdate(){
        if(!initialized) return;
        transform.position = initialPosition;
        transform.up = originalUp;
    }

    protected virtual void OnTriggerEnter2D(Collider2D otherCollider){
        ItemEffect otherEffect = otherCollider.gameObject.GetComponent<ItemEffect>();
        CarController car = otherCollider.gameObject.GetComponent<CarController>();
        if(otherEffect != null){
            if(otherEffect.GetType() == typeof(Missile)) Destroy(otherEffect.gameObject);
            Destroy(this.gameObject);
            return;
        }
        Vector2 explosionDirection = (car.transform.position - transform.position).normalized;
        Rigidbody2D carRigidbody = car.GetComponent<Rigidbody2D>();
        car.ChangeState(new LossOfControlState(car.State, lossOfControlTime));
        car.ApplyDamage(damage);
        carRigidbody.velocity = Vector2.zero;
        carRigidbody.AddForce(explosionDirection * power * car.Statistics.ejectionRate, ForceMode2D.Impulse);
        Destroy(this.gameObject);
    }

}