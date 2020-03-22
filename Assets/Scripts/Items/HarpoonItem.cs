using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonItem : Item {

    public float Range{get; private set;}

    public HarpoonItem(CarController Owner) : base(Owner) {
        Name = "harpoon";
        Cooldown = 2;
        Range = 12;
    }

    protected override void Active(){
        GameObject harpoonObject = GetInstantiatedPrefab();
        Harpoon harpoon = harpoonObject.GetComponent<Harpoon>();

        harpoon.SetOwner(Owner);
        harpoon.SetRange(Range);
        harpoon.SetTarget(Owner.GetAimedPositon());
    }

}