using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileItem : Item {

    public MissileItem(CarController Owner) : base(Owner) {
        Name = "missile";
        Cooldown = 5;
    }

    protected override void Active(){
        GameObject missileObject = GetInstantiatedPrefab();
        Missile missile = missileObject.GetComponent<Missile>();
        missile.SetOwner(Owner);
        missile.SetTarget(Owner.GetAimedPositon());
    }

}