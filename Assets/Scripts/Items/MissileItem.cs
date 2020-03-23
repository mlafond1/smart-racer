using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileItem : Item {

    public MissileItem(CarController Owner) : base(Owner) {
        Name = "missile";
        Cooldown = 5;
    }

}