using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    public CarController Owner{get;set;}
    public float Cooldown{get; protected set;}
    private bool isReady{get; set;}

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

    private IEnumerator WaitCooldown(){
        yield return new WaitForSeconds(Cooldown);
        isReady = true;
    }

}