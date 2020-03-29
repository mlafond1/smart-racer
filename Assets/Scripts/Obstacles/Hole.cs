using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : Obstacle
{
    float duration = 5f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        this.Comportement(other);
    }
    public override void Comportement(Collider2D collider)
    {
        CarController car = collider.gameObject.GetComponent<CarController>();
        if (car != null)
        {
            //car.ChangeState(new FallState (car.State, duration, this.transform.position));
            car.Respawn();
        }
    }
}