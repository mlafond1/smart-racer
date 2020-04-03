using UnityEngine;

public class Surface : Obstacle
{
    // Higher friction = slower
    [SerializeField]
    private float accelerationDampener = 2f;
    [SerializeField]
    private float friction = 0f;
    [SerializeField]
    private float trailDuration = 0f;
    private SurfaceState surface = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CarController car = other.gameObject.GetComponent<CarController>();
        if (car != null)
        {
            Debug.Log("Enter trigger");
            surface = new SurfaceState(car.State, accelerationDampener, friction, trailDuration, this);
            car.ChangeState(surface);
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit trigger");
        CarController car = other.gameObject.GetComponent<CarController>();
        surface.OnStateExit();
    }
}