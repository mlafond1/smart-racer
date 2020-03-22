using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeItem : Item {

    public BladeItem(CarController Owner) : base(Owner) {
        Name = "blade";
        Cooldown = 5;
    }

    protected override void Active(){
        GameObject bladeObject = GetInstantiatedPrefab();
        Blade blade = bladeObject.GetComponent<Blade>();

        blade.SetOwner(Owner);
    }

}