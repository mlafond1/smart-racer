using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonItem : Item {

    public float Range{get; private set;}

    public HarpoonItem(CarController Owner) : base(Owner) {
        Name = "harpoon";
        Cooldown = 10;
        Range = 12;
    }

}