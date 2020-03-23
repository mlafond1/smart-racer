using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineItem : Item {

    public MineItem(CarController Owner) : base(Owner) {
        Name = "mine";
        Cooldown = 7;
    }

}