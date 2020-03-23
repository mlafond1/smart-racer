using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : ItemEffect {

    Vector3 target;
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
    }

    public override void InitialSetup(Item item){
        MissileItem missileItem = (MissileItem)item;
        SetOwner(item.Owner);
        SetTarget(item.Owner.GetAimedPositon());
        transform.up = target - transform.position;
        originalUp = transform.up;
        launchVelocity = owner.gameObject.GetComponent<Rigidbody2D>().velocity;
        launchSpeedDuration = maxLaunchSpeedDuration;
        initialized = true;
    }  

    void FixedUpdate(){
        if(!initialized) return;
        transform.up = originalUp;
        rb.velocity = originalUp * speed;
        rb.velocity += launchSpeedDuration > 0 ? launchVelocity * (launchSpeedDuration/maxLaunchSpeedDuration) : Vector2.zero;
        rb.angularVelocity = 0;

        launchSpeedDuration -= Time.deltaTime;
    }

    public void SetTarget(Vector3 target){
        this.target = target;
    }

    public override void OnReflect(CarController other){
        Collider2D effectCollider = this.gameObject.GetComponent<Collider2D>();
        Collider2D previousOwnerCollider = owner.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(previousOwnerCollider, effectCollider, false); // Peut rentrer en collision avec le précédent propriétaire
        SetOwner(other);
        originalUp = -originalUp;
        launchVelocity = other.GetComponent<Rigidbody2D>().velocity;
        // Réactiver les collisions aux autres objets
        ItemEffect[] effects = GameObject.FindObjectsOfType<ItemEffect>();
        foreach(var effect in effects){
            Physics2D.IgnoreCollision(effectCollider, effect.gameObject.GetComponent<Collider2D>(), false);
        }
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