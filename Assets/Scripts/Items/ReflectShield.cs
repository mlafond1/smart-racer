using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectShield : Shield {

    new public const string Name = "reflect";
    const float cooldown = 7;
    const float duration = 3;

    public override void InitialSetup(Item item){
        InitialSetup(item, duration);
    }

    public override float GetCooldown(){
        return cooldown;
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