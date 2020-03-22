using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : ItemEffect {

    Vector3 target;
    float range;
    bool initialized = false;
    bool isPulling = false;
    LineRenderer rope;
    Vector2 launchVelocity;
    Vector3 originalUp;
    Rigidbody2D rb;

    float maxLaunchSpeedDuration = 2f;
    float launchSpeedDuration = 0f;

    float speed = 40;
    float damage = 7;
    float lossOfControlTime = 1f;

    void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        rope = gameObject.GetComponent<LineRenderer>();
        initialized = false;
    }

    void Update(){
        rope.positionCount = 2;
        rope.SetPosition(0, owner.transform.position);
        rope.SetPosition(1, transform.position);
    }

    void FixedUpdate(){
        if(target == null || owner == null || range == 0) return;
        if(!initialized){
            transform.up = target - transform.position;
            originalUp = transform.up;
            initialized = true;
            launchVelocity = owner.gameObject.GetComponent<Rigidbody2D>().velocity;
            launchSpeedDuration = maxLaunchSpeedDuration;
        }
        if(!isPulling){
            transform.up = originalUp;
            rb.velocity = originalUp * speed;
            rb.velocity += launchSpeedDuration > 0 ? launchVelocity * (launchSpeedDuration/maxLaunchSpeedDuration) : Vector2.zero;
            launchSpeedDuration -= Time.deltaTime;
            if(Vector3.Distance(owner.transform.position, transform.position) >= range) Destroy(this.gameObject);
        }
        rb.angularVelocity = 0;

    }

    public void SetTarget(Vector3 target){
        this.target = target;
    }

    public void SetRange(float range){
        this.range = range;
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(isPulling) return;
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
        car.ChangeState(new LossOfControlState(car.State, lossOfControlTime));
        car.ApplyDamage(damage);
        collision.rigidbody.velocity = Vector2.zero;
        StartCoroutine(PerformPullBack(car));
    }

    IEnumerator PerformPullBack(CarController car){
        isPulling = true;
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        Collider2D ownerCollider = owner.GetComponent<Collider2D>();
        Collider2D otherCollider = car.GetComponent<Collider2D>();
        Rigidbody2D otherRb = car.GetComponent<Rigidbody2D>();
        float duration = 0;
        transform.position += transform.up.normalized/2f; // Bien ancré
        while(isPulling){
            if(ownerCollider.Distance(otherCollider).distance < 2f || duration >= lossOfControlTime) break;
            yield return new WaitForFixedUpdate();
            Vector3 ownerPosition = owner.transform.position;
            Vector3 harpoonPosition = transform.position;
            Vector3 pullDirection = (ownerPosition - harpoonPosition).normalized;
            pullDirection = (pullDirection/4f) * (1.5f + duration);
            otherCollider.transform.position += pullDirection;
            transform.position += pullDirection;
            transform.up = -pullDirection;
            duration += Time.fixedDeltaTime;
        }
        car.State.ClearDuration();
        otherRb.AddForce(-otherRb.transform.up.normalized/2, ForceMode2D.Impulse);
        Destroy(this.gameObject);
    }

}