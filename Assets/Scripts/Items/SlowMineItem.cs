using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMineItem : Item {

    public float Duration{get;set;}

    public SlowMineItem(CarController Owner) : base(Owner) {
        Name = "slow-mine";
        Cooldown = 7;
        Duration = 2;
    }

}