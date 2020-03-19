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
    float driftPourcentage = 0.95f;

    Vector3 aimedPosition;
    public Collider2D respawnpoint = null;//TODO remettre private

    Item[] items = new Item[2];

    public CarStatistics Statistics{get; private set;}
    public CarState State{get; private set;}

    void Awake()
    {
        this.Statistics = new CarStatistics(maxSpeed, torqueSpeed, driftPourcentage);
        this.State = new OnTrackState(this);
        SetItem(0, new MissileItem(this)); // TODO TEMP
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
        return items[index];
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

    public void ChangeState(CarState newState){
        if(!this.enabled) return;
        if(State.CanChangeState(newState)){
            State = newState;
        }
    }

    public void SetRespawnpoint(Collider2D newRespawnpoint){
        this.respawnpoint = newRespawnpoint;
    }

    public void Respawn(){
        this.transform.position = this.respawnpoint.transform.position;
    }

    public void Respawn(Collider2D respawnpoint_){
        this.transform.position = respawnpoint_.transform.position;
    }
}
