using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : ItemEffect {

    const float rotationOffset = -134.353f;
    bool isDoneStabbing = false;
    float distanceFromCenter = 0f;
    float angleRotation = 0;
    Rigidbody2D rb;

    float range = 2;
    float speed = 20;
    float power = 30;
    float damage = 20;
    float lossOfControlTime = 0.3f;

    void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public override void InitialSetup(Item item){
        BladeItem bladeItem = (BladeItem)item;
        SetOwner(bladeItem.Owner);
        initialized = true;
    }

    void Update(){
        if(!initialized) return;
        transform.up = owner.transform.up;
        transform.Rotate(Vector3.forward, rotationOffset);
        transform.position = owner.transform.position + owner.transform.up.normalized * distanceFromCenter;
        // Spin
        if(isDoneStabbing){
            transform.RotateAround(owner.transform.position, Vector3.forward, angleRotation);
            angleRotation += Time.deltaTime * speed * speed * 3;
            if(angleRotation >= 375) Destroy(this.gameObject);
        }
    }

    void FixedUpdate(){
        if(!initialized) return;
        // Stab
        if(!isDoneStabbing){
            distanceFromCenter += Time.fixedDeltaTime * speed;
            if(distanceFromCenter >= range){
                transform.position = owner.transform.position + owner.transform.up.normalized * range;
                distanceFromCenter = range;
                isDoneStabbing = true;
            }
        }

        rb.angularVelocity = 0;
    }

    void OnCollisionEnter2D(Collision2D collision){
        ItemEffect otherEffect = collision.collider.gameObject.GetComponent<ItemEffect>();
        CarController car = collision.collider.gameObject.GetComponent<CarController>();
        bool isEffectAShield = 
            otherEffect != null && 
            (otherEffect.GetType().IsSubclassOf(typeof(Shield)) || otherEffect.GetType() == typeof(Shield));
        if(owner.Equals(car) || SameOwner(otherEffect) || isEffectAShield){
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            return;
        }
        if(car == null){
            Destroy(this.gameObject);
            return;
        }
        ContactPoint2D contactPoint = collision.GetContact(0);
        Vector2 swordDirection = (transform.position - owner.transform.position).normalized;
        
        car.ChangeState(new LossOfControlState(car.State, lossOfControlTime));
        car.ApplyDamage(damage);
        collision.rigidbody.velocity = Vector2.zero;
        collision.rigidbody.AddForceAtPosition(swordDirection * power * car.Statistics.ejectionRate, contactPoint.point, ForceMode2D.Impulse);
        Physics2D.IgnoreCollision(collision.collider, collision.otherCollider); // Pas appliquer 2 fois les dégâts
    }

}