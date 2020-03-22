using UnityEngine;

public class Ramp : Obstacle
{
    // Height of ramp
    [SerializeField]
    private float height = 1f;

    // Angle of ramp
    [SerializeField]
    private float angle = 15f;

    // Gravitational constant
    [SerializeField]
    private float g = -9.8f;
    
    private void OnTriggerExit2D(Collider2D other) {
        this.Comportement(other);
    }
    public override void Comportement(Collider2D collider)
    {
        CarController car = collider.gameObject.GetComponent<CarController>();
        if (car != null)
        {
           float duration = CalcDuration(car.State.CurrentSpeed());
           car.ChangeState(new JumpState(car, duration));
        }
    }

    private float CalcDuration(float speed){
        // 0 = height + (speed*cos(angle)) * t + 1/2*g*t^2
        // 0 = 1/2*g*t^2 + (speed*cos(angle)) * t + height
        // 0 = a*t^2 + b*t + height
        float a = 0.5f * g;
        float b = speed * Mathf.Sin(Mathf.Deg2Rad * angle);
        float c = height;
        float delta = Mathf.Sqrt((b*b) - (4*a*c));
        float x1 = (-b + delta)/(2*a);
        float x2 = (-b - delta)/(2*a);

        return Mathf.Max(x1, x2);
    }
}