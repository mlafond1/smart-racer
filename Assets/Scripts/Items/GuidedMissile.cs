using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedMissile : ItemEffect {

    public const string Name = "guided-missile";
    const float cooldown = 7;

    float speed = 30;
    float power = 12;
    float damage = 15;
    float lossOfControlTime = 0.3f;

    bool foundFirstCheckpoint = false;
    bool hasTargetLocked = false;
    bool isGoingForward;
    int checkpointIndex = 0;
    CarController lockedCar;
    List<Collider2D> checkpoints;
    List<CarController> cars;
    Vector3 originalUp;
    Vector2 launchVelocity;
    Rigidbody2D rb;
    Collider2D missileCollider;

    float maxLaunchSpeedDuration = 2f;
    float launchSpeedDuration = 0f;

    void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        missileCollider = gameObject.GetComponent<Collider2D>();
        checkpoints = new List<Collider2D>(GameObject.Find("AI Checkpoints").GetComponentsInChildren<Collider2D>());
        cars = new List<CarController>(Resources.FindObjectsOfTypeAll<CarController>());
    }

    public override void InitialSetup(Item item){
        SetOwner(item.Owner);
        Vector3 aimedPosition = owner.GetAimedPositon();
        isGoingForward = 
            Vector3.Distance(aimedPosition, transform.position + transform.up) <
            Vector3.Distance(aimedPosition, transform.position - transform.up);
        originalUp = isGoingForward ? owner.transform.up : -owner.transform.up;
        transform.up = originalUp;
        launchVelocity = owner.gameObject.GetComponent<Rigidbody2D>().velocity;
        launchSpeedDuration = maxLaunchSpeedDuration;
        initialized = true;
    }

    public override float GetCooldown(){
        return cooldown;
    }

    private void CycleIndex(){
        checkpointIndex += isGoingForward ? 1 : -1;
        if(checkpointIndex <= -1) checkpointIndex = checkpoints.Count-1;
        else if(checkpointIndex >= checkpoints.Count) checkpointIndex = 0;
    }

    void FixedUpdate(){
        if(!initialized) return;
        if(!foundFirstCheckpoint){ // Va tout droit jusqu'au premier checkpoint
            transform.up = originalUp;
            rb.velocity = originalUp * speed;
            rb.velocity += launchSpeedDuration > 0 ? launchVelocity * (launchSpeedDuration/maxLaunchSpeedDuration) : Vector2.zero;
            checkpointIndex = 0;
            foreach(var checkpoint in checkpoints){
                if(missileCollider.IsTouching(checkpoint)){
                    foundFirstCheckpoint = true;
                    break;
                }
                ++checkpointIndex;
            }
            CycleIndex();
        }
        else { // Va jusqu'au prochain checkpoint
            Vector2 direction = checkpoints[checkpointIndex].transform.position - transform.position;
            transform.up = direction;
            rb.velocity = transform.up * speed;
            if(missileCollider.IsTouching(checkpoints[checkpointIndex])){
                CycleIndex();
            }
        }
        // Vise la voiture la plus proche si suffisament proche
        if(hasTargetLocked){
            Vector2 direction = lockedCar.transform.position - transform.position;
            transform.up = direction;
            rb.velocity = transform.up * speed;
        }
        else {
            float minDistance = float.PositiveInfinity;
            foreach(var car in cars){
                if(car.Equals(owner)) continue;
                float distanceAhead = Vector2.Distance(car.transform.position, transform.position + transform.up);
                Vector2 direction = car.transform.position - transform.position;
                minDistance = distanceAhead < minDistance ? distanceAhead : minDistance;
                if(distanceAhead < 5 && minDistance.Equals(distanceAhead)){
                    transform.up = direction;
                    rb.velocity = transform.up * speed;
                    lockedCar = car;
                    hasTargetLocked = true;
                }
            }
        }
        
        rb.angularVelocity = 0;

        launchSpeedDuration -= Time.deltaTime;
    }

    public override void OnReflect(CarController other){
        Collider2D previousOwnerCollider = owner.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(previousOwnerCollider, missileCollider, false); // Peut rentrer en collision avec le précédent propriétaire
        SetOwner(other);
        originalUp = -originalUp;
        launchVelocity = other.GetComponent<Rigidbody2D>().velocity;
        isGoingForward = !isGoingForward;
        hasTargetLocked = false;
        CycleIndex();
        // Réactiver les collisions aux autres objets
        ItemEffect[] effects = GameObject.FindObjectsOfType<ItemEffect>();
        foreach(var effect in effects){
            Physics2D.IgnoreCollision(missileCollider, effect.gameObject.GetComponent<Collider2D>(), false);
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