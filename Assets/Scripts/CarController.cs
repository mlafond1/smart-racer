using System.Collections;
using System.Collections.Generic;
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

    Item[] items = new Item[2];

    public CarStatistics Statistics{get; private set;}
    public CarState State{get; private set;}

    void Awake()
    {
        this.Statistics = new CarStatistics(maxSpeed, torqueSpeed, driftPercentage, power, attackDamage);
        this.State = new OnTrackState(this);
        SetItem(0, new HarpoonItem(this)); // TODO TEMP
        SetItem(1, new ShieldItem(this)); // TODO TEMP
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
            State = newState;
        }
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
