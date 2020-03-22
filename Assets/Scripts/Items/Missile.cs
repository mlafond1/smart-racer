using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : ItemEffect {

    Vector3 target;
    bool initialized = false;
    Vector2 launchVelocity;
    Vector3 originalUp;
    Rigidbody2D rb;

    float maxLaunchSpeedDuration = 2f;
    float launchSpeedDuration = 0f;

    float speed = 20;
    float power = 12;
    float damage = 15;
    float lossOfControlTime = 0.3f;

    void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        initialized = false;
    }

    void FixedUpdate(){
        if(target == null || owner == null) return;
        if(!initialized){
            transform.up = target - transform.position;
            originalUp = transform.up;
            initialized = true;
            launchVelocity = owner.gameObject.GetComponent<Rigidbody2D>().velocity;
            launchSpeedDuration = maxLaunchSpeedDuration;
        }
        transform.up = originalUp;
        rb.velocity = originalUp * speed;
        rb.velocity += launchSpeedDuration > 0 ? launchVelocity * (launchSpeedDuration/maxLaunchSpeedDuration) : Vector2.zero;
        rb.angularVelocity = 0;

        launchSpeedDuration -= Time.deltaTime;
    }

    public void SetTarget(Vector3 target){
        this.target = target;
    }

    void OnCollisionEnter2D(Collision2D collision){
        ItemEffect otherEffect = collision.collider.gameObject.GetComponent<ItemEffect>();
        CarController car = collision.collider.gameObject.GetComponent<CarController>();
        if(owner.Equals(car) || SameOwner(otherEffect)){
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            return;
        }
        if(car == null){
            Destroy(this.gameObject);
            return;
        }
        ContactPoint2D contactPoint = collision.GetContact(0);
        Vector2 missileDirection = transform.up;
        car.ChangeState(new LossOfControlState(car.State, lossOfControlTime));
        car.ApplyDamage(damage);
        collision.rigidbody.velocity = Vector2.zero;
        collision.rigidbody.AddForceAtPosition(missileDirection * power * car.Statistics.ejectionRate, contactPoint.point, ForceMode2D.Impulse);
        Destroy(this.gameObject);
    }

}