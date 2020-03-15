using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : Item {

    public int Duration{get;set;}

    public ShieldItem(CarController Owner) : base(Owner) {
        Name = "shield";
        Cooldown = 10;
        Duration = 5;
    }

    protected override void Active(){
        // Creer effet bouclier
        GameObject shield = GetInstantiatedPrefab();
        shield.transform.SetParent(Owner.transform);
        // Garder actif pour Duration secondes
        Owner.StartCoroutine(WaitDuration(shield));
    }

    private IEnumerator WaitDuration(GameObject shield){
        yield return new WaitForSeconds(Duration);
        GameObject.DestroyImmediate(shield);
    }

}