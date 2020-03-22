using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectShieldItem : Item {

    public float Duration{get;set;}

    public ReflectShieldItem(CarController Owner) : base(Owner) {
        Name = "reflect";
        Cooldown = 7;
        Duration = 3;
    }

    protected override void Active(){
        GameObject reflectObject = GetInstantiatedPrefab();
        ReflectShield shield = reflectObject.GetComponent<ReflectShield>();

        shield.SetDuration(Duration);
        shield.SetOwner(Owner);
    }

}