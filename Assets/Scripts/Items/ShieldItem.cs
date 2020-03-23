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

}