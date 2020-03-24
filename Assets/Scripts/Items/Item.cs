using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public CarController Owner{get; private set;}
    public string Name{get; protected set;}
    public float Cooldown{get; protected set;}
    public float CooldownTimer{get; private set;}
    public bool isReady{get; private set;}

    public Item(CarController Owner){ // TODO delete
        this.Owner = Owner;
        this.isReady = true;
    }

    public Item(CarController Owner, string Name){
        this.Owner = Owner;
        this.Name = Name;
        this.isReady = true;
        this.Cooldown = GetPrefab().GetComponent<ItemEffect>().GetCooldown();
    }

    public void Use(){
        if(isReady){
            isReady = false;
            Active();
            Owner.StartCoroutine(WaitCooldown());
        }
    }

    protected virtual void Active(){
        GameObject itemEffectObject = GetInstantiatedPrefab();
        ItemEffect effect = itemEffectObject.GetComponent<ItemEffect>();
        effect.InitialSetup(this);
    }

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

    public void ForceWaitCooldown(){
        isReady = false;
        Owner.StartCoroutine(WaitCooldown());
    }

}