using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : ItemEffect {

    Vector3 target;
    bool initialized = false;
    Vector2 launchVelocity;
    Vector3 originalUp;
    Rigidbody2D rb;

    float speed = 15;
    float power = 15;

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
        }
        transform.up = originalUp;
        rb.velocity = originalUp * speed;
        rb.velocity += launchVelocity;
        rb.angularVelocity = 0;
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
        collision.rigidbody.AddForceAtPosition(missileDirection * power, contactPoint.point, ForceMode2D.Impulse);
        Destroy(this.gameObject);
    }

}