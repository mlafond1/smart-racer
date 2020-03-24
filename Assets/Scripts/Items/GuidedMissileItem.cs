using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedMissileItem : Item {

    public GuidedMissileItem(CarController Owner) : base(Owner) {
        Name = "guided-missile";
        Cooldown = 7;
    }

}