using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostItem : Item {

    public float Duration{get; private set;}
    public float PowerIncrease{get; private set;}
    public float DamageIncrease{get; private set;}

    public BoostItem(CarController Owner) : base(Owner) {
        Name = "boost";
        Cooldown = 6;
        Duration = 3;
        PowerIncrease = 8;
        DamageIncrease = 8;
    }

}