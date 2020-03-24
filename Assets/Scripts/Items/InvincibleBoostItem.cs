using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleBoostItem : Item {

    public float Duration{get; private set;}
    public float PowerIncrease{get; private set;}
    public float DamageIncrease{get; private set;}

    public InvincibleBoostItem(CarController Owner) : base(Owner) {
        Name = "invincible-boost";
        Cooldown = 12;
        Duration = 4;
        PowerIncrease = 10;
        DamageIncrease = 10;
    }

}