using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : Obstacle
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        this.Comportement(other);
    }
    public override void Comportement(Collider2D collider)
    {
        CarController car = collider.gameObject.GetComponent<CarController>();
        if (car != null)
        {
            car.Respawn();
        }
    }
}