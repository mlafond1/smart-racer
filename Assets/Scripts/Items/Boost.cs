using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : ItemEffect {

    void Start(){
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void InitialSetup(Item item){
        BoostItem boostItem = (BoostItem)item;
        SetOwner(boostItem.Owner);
        owner.ChangeState(new BoostedState(owner.State, boostItem.Duration, boostItem.PowerIncrease, boostItem.DamageIncrease));
        Destroy(this.gameObject);
    }

}