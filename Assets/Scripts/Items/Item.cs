using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item {

    public CarController Owner{get;set;}
    public string Name{get; protected set;}
    public float Cooldown{get; protected set;}
    private bool isReady{get; set;}

    public Item(CarController Owner){
        this.Owner = Owner;
    }

    void Start(){
        isReady = true;
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

    protected GameObject GetInstantiatedPrefab(){
        Vector3 position = Owner.transform.position;
        Quaternion rotation = Owner.transform.rotation;

        return GameObject.Instantiate(GetPrefab(), position, rotation);
    }

    private IEnumerator WaitCooldown(){
        yield return new WaitForSeconds(Cooldown);
        isReady = true;
    }

}