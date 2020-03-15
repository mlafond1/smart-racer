using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileItem : Item {

    public MissileItem(CarController Owner) : base(Owner) {
        Name = "missile";
        Cooldown = 10;
    }

    protected override void Active(){
        // Direction choisie en fonction de l'emplacement de la souris
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)){
            // Creer missile Effect
            GameObject missile = GetInstantiatedPrefab();
            // Orienter le missile vers l'emplacement du clic
            Vector3 direction = hit.point - missile.transform.position;
            Quaternion rotationToward = Quaternion.LookRotation(direction);
            missile.transform.rotation = rotationToward;
        }
    }

}