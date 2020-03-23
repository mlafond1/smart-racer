using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ItemEffect{

    float duration;
    protected Collider2D ownerCollider;

    public override void InitialSetup(Item item){
        ShieldItem shieldItem = (ShieldItem)item;
        InitialSetup(shieldItem, shieldItem.Duration);
    }

    protected void InitialSetup(Item item, float shieldDuration){
        SetOwner(item.Owner);
        SetDuration(shieldDuration);
        ownerCollider = owner.gameObject.GetComponent<Collider2D>();
        owner.Statistics.ToggleDamage(false);
        initialized = true;
    }

    void Update(){
        if(!initialized) return;
        duration -= Time.deltaTime;
        if(duration <= 0){
            ToggleOtherEffects(false);
            owner.Statistics.ToggleDamage(true);
            Destroy(this.gameObject);
        }
        transform.position = owner.transform.position;
        transform.up = owner.transform.up;
    }
    
    void FixedUpdate(){
        if(!initialized) return;
        transform.position = owner.transform.position;
        if(duration > 0){
            ToggleOtherEffects(true);
        }
    }

    public void SetDuration(float duration){
        this.duration = duration;
    }

    protected virtual void ToggleOtherEffects(bool ignore){
        ItemEffect[] effects = GameObject.FindObjectsOfType<ItemEffect>();
        foreach(var effect in effects){
            Physics2D.IgnoreCollision(ownerCollider, effect.gameObject.GetComponent<Collider2D>(), ignore);
        }
    }

    // TODO Refactor avec CarController.OnCollisionEnter2D...??
    protected virtual void OnCollisionEnter2D(Collision2D collision){
        CarController otherCar;
        if(collision.collider.TryGetComponent<CarController>(out otherCar)){
            // Damage
            float dotProduct = Vector2.Dot(otherCar.transform.right, owner.transform.up);
            bool isDamagingCollision = Mathf.Abs(dotProduct) > 0.15;
            if(isDamagingCollision) otherCar.ApplyDamage(Mathf.Abs(dotProduct) * owner.Statistics.attackDamage * owner.PercentOfMaxSpeed());
            // Bump
            Vector3 directionAway = otherCar.transform.position - transform.position;
            directionAway.z = 0;
            otherCar.ChangeState(new LossOfControlState(otherCar.State, 0.07f));
            collision.rigidbody.AddForce(directionAway.normalized * owner.Statistics.power * (isDamagingCollision ? otherCar.Statistics.ejectionRate : 1), ForceMode2D.Impulse);
        }
    }

}