using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item {

    public CarController Owner{get; private set;}
    public string Name{get; protected set;}
    public float Cooldown{get; protected set;}
    public float CooldownTimer{get; private set;}
    public bool isReady{get; private set;}

    public Item(CarController Owner){
        this.Owner = Owner;
        this.isReady = true;
    }

    public void Use(){
        if(isReady){
            isReady = false;
            Active();
            Owner.StartCoroutine(WaitCooldown());
        }
    }

    protected abstract void Active();

    protected GameObject GetPrefab(){
        return LoadItems.GetItemPrefab(Name);
    }

    public Sprite GetSprite(){
        return LoadItems.GetItemPrefab(Name)?.GetComponent<SpriteRenderer>()?.sprite;
    }

    protected GameObject GetInstantiatedPrefab(){
        Vector3 position = Owner.transform.position;
        Quaternion rotation = Owner.transform.rotation;

        return GameObject.Instantiate(GetPrefab(), position, rotation);
    }

    private IEnumerator WaitCooldown(){
        CooldownTimer = Cooldown;
        while(CooldownTimer > 0){
            float decrement = CooldownTimer > 1 ? 1 : CooldownTimer;
            yield return new WaitForSeconds(decrement);
            CooldownTimer -= decrement;
        }
        
        isReady = true;
    }

}