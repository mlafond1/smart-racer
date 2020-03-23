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

}