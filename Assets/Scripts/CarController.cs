using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class CarController : MonoBehaviour
{

    // Pour changer les statistiques dans l'éditeur
    [SerializeField]
    float maxSpeed = 30f;
    [SerializeField]
    float torqueSpeed = 6f;
    [SerializeField]
    float driftPercentage = 0.95f;
    [SerializeField]
    float power = 7f;
    [SerializeField]
    float attackDamage = 10f;

    Vector3 aimedPosition;
    public Collider2D respawnpoint = null;//TODO remettre private
    public int offTrackCounter;

    Item[] items = new Item[2];

    public CarStatistics Statistics{get; private set;}
    public CarState State{get; private set;}

    // TEMP cycler à travers les items
    Item[] offensiveItems;
    int offensiveItemsIndex = 0;
    Item[] defensiveItems;
    int defensiveItemsIndex = 0;

    void Awake()
    {
        this.Statistics = new CarStatistics(maxSpeed, torqueSpeed, driftPercentage, power, attackDamage);
        this.State = new OnTrackState(this);
        // TEMP cycler à travers les items
        offensiveItems = new Item[] { 
            new Item(this, Missile.Name),
            new Item(this, GuidedMissile.Name),
            new Item(this, Blade.Name), 
            new Item(this, Harpoon.Name),
            new Item(this, Mine.Name),
            new Item(this, SlowMine.Name)
        };
        defensiveItems = new Item[] { 
            new Item(this, Shield.Name), 
            new Item(this, ReflectShield.Name),
            new Item(this, Ghost.Name),
            new Item(this, Boost.Name),
            new Item(this, InvincibleBoost.Name)
        };
        SetItem(0, offensiveItems[0]);
        SetItem(1, defensiveItems[0]);
        offTrackCounter = 0;
    }

    public void Accelerate(){
        if(!this.enabled) return;
        State.Accelerate();
    }

    public void Brake(){
        if(!this.enabled) return;
        State.Brake();
    }

    public void Steer(float horizontalAxis){
        State.Steer(horizontalAxis);
    }

    public void SetItem(int index, Item item){
        items[index] = item;
    }

    public Item GetItem(int index){
        return index >= items.Length ? null : items[index];
    }

    public void UseItem(int index){
        if(!this.enabled) return;
        items[index]?.Use();
    }

    public void Aim(Vector3 position){
        this.aimedPosition = position;
    }

    public Vector3 GetAimedPositon(){
        return this.aimedPosition;
    }

    void Update(){
        // TEMP cycler à travers les items
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            offensiveItemsIndex = (offensiveItemsIndex + 1) % offensiveItems.Length;
            SetItem(0, offensiveItems[offensiveItemsIndex]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            defensiveItemsIndex = (defensiveItemsIndex + 1) % defensiveItems.Length;
            SetItem(1, defensiveItems[defensiveItemsIndex]);
        }
    }

    void FixedUpdate()
    {
        State.Drive();
    }

    public float CurrentSpeed(){
        return State.CurrentSpeed();
    }

    public float PercentOfMaxSpeed(){
        return State.PercentOfMaxSpeed();
    }

    public void ApplyDamage(float damage){
        Statistics.ApplyDamage(damage);
    }

    public void ChangeState(CarState newState){
        if(State.CanChangeState(newState)){
            // ** Debug.Log(State.GetType() + ":before");
            State = newState;
            // if(State.GetType() == typeof(OffTrackState)){
            //     offTrackCounter++;
            //     Debug.Log(offTrackCounter);
            // }
            State.OnStateEnter();
            // ** Debug.Log(State.GetType() + ":after");
        }
    }

    public void SetRespawnpoint(Collider2D newRespawnpoint){
        this.respawnpoint = newRespawnpoint;
    }

    // TODO face front
    public void Respawn(){
        Vector3 tmp = this.transform.position;
        tmp.x = this.respawnpoint.transform.position.x;
        tmp.y = this.respawnpoint.transform.position.y;
        this.transform.position = tmp;
        this.transform.up = this.respawnpoint.transform.up;
    }

    public void Respawn(Collider2D respawnpoint_){
        Vector3 tmp = this.transform.position;
        tmp.x = respawnpoint_.transform.position.x;
        tmp.y = respawnpoint_.transform.position.y;
        this.transform.position = tmp;
        this.transform.up = this.respawnpoint.transform.up;
    }
    void OnCollisionEnter2D(Collision2D collision){
        CarController otherCar;
        if(collision.collider.TryGetComponent<CarController>(out otherCar)){
            // Damage
            float dotProduct = Vector2.Dot(otherCar.transform.right, transform.up);
            List<RaycastHit2D> objectsInFront = new List<RaycastHit2D>(Physics2D.RaycastAll(transform.position, transform.up, 5f));
            bool isOtherCarInFront = objectsInFront.Find((obj) => obj.collider.Equals(collision.collider));
            bool isDamagingCollision = Mathf.Abs(dotProduct) > 0.15 && isOtherCarInFront;
            if(isDamagingCollision) otherCar.ApplyDamage(Mathf.Abs(dotProduct) * Statistics.attackDamage * PercentOfMaxSpeed());
            // Bump
            Vector3 directionAway = otherCar.transform.position - transform.position;
            directionAway.z = 0;
            otherCar.ChangeState(new LossOfControlState(otherCar.State, 0.07f));
            collision.rigidbody.AddForce(directionAway.normalized * Statistics.power * (isDamagingCollision ? otherCar.Statistics.ejectionRate : 1), ForceMode2D.Impulse);
        }
        // Explosion
        bool isHittingAnObstacle = otherCar == null && !collision.collider.GetComponent<ItemEffect>();
        bool isAtHighSpeed = PercentOfMaxSpeed() > 0.75f;
        bool isAtHighDamage = Statistics.ejectionRate >= 2;
        bool isAtLossOfControl = State.GetType() == typeof(LossOfControlState);
        if(isHittingAnObstacle && isAtHighSpeed && (isAtHighDamage || isAtLossOfControl)){
            Debug.Log("Explosion -> " + this.name);
        }
    }

}
