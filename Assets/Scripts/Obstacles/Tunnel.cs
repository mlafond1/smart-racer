using UnityEngine;

public class Tunnel : Obstacle
{
    // // Height of ramp
    // [SerializeField]
    // private float height = 1f;

    // // Angle of ramp
    // [SerializeField]
    // private float angle = 15f;

    // // Gravitational constant
    // [SerializeField]
    // private float g = -9.8f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        this.Comportement(other);
    }
    public override void Comportement(Collider2D collider)
    {
        CarController car = collider.gameObject.GetComponent<CarController>();
        if (car != null)
        {
            
        }
    }

}