using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : Item {

    public float Duration{get;set;}

    public ShieldItem(CarController Owner) : base(Owner) {
        Name = "shield";
        Cooldown = 6;
        Duration = 3;
    }

    protected override void Active(){
        GameObject shieldObject = GetInstantiatedPrefab();
        Shield shield = shieldObject.GetComponent<Shield>();

        shield.SetDuration(Duration);
        shield.SetOwner(Owner);
    }

}