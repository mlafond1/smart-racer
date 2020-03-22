using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostItem : Item {

    public float Duration{get;set;}

    public GhostItem(CarController Owner) : base(Owner) {
        Name = "ghost";
        Cooldown = 8;
        Duration = 4;
    }

    protected override void Active(){
        GameObject ghostObject = GetInstantiatedPrefab();
        Ghost ghost = ghostObject.GetComponent<Ghost>();

        ghost.SetDuration(Duration);
        ghost.SetOwner(Owner);
    }

}