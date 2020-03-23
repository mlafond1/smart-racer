using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectShield : Shield {

    public override void InitialSetup(Item item){
        ReflectShieldItem reflectShieldItem = (ReflectShieldItem)item;
        InitialSetup(reflectShieldItem, reflectShieldItem.Duration);
    }

    protected override void OnCollisionEnter2D(Collision2D collision){
        base.OnCollisionEnter2D(collision);
        ItemEffect otherEffect;
        if(collision.collider.TryGetComponent<ItemEffect>(out otherEffect)){
            if(SameOwner(otherEffect)) return;
            otherEffect.OnReflect(owner);
        }
    }

}