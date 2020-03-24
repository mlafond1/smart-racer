using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : ItemEffect {

    public const string Name = "boost";
    const float cooldown = 6;
    const float duration = 3;
    const float powerIncrease = 8;
    const float damageIncrease = 8;


    void Start(){
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void InitialSetup(Item item){
        SetOwner(item.Owner);
        owner.ChangeState(new BoostedState(owner.State, duration, powerIncrease, damageIncrease));
        Destroy(this.gameObject);
    }

    public override float GetCooldown(){
        return cooldown;
    }

}