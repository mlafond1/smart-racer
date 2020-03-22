using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    float distanceAhead = 5;

    private int carIndex = 0;

    private CarController carController;
    private List<CarController> cars;

    Vector3 offset = new Vector3(0,0, -10); // La caméra doit être au dessus du véhicule pour le voir

    void Start()
    {
        cars = new List<CarController>(Resources.FindObjectsOfTypeAll<CarController>());
        carController = GetComponentInParent<CarController>();
        if(carController != null){
            carIndex = cars.FindIndex((other) => other.Equals(carController));
        }
        else {
            carIndex = 0;
            carController = cars[carIndex];
        }
    }

    void SwitchCar(){
        carIndex = (carIndex+1) % cars.Count;
        carController = cars[carIndex];
        transform.SetParent(carController.gameObject.transform);
        transform.position = transform.parent.position + offset;
        transform.rotation = transform.parent.rotation;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            SwitchCar();
        }
    }

    void FixedUpdate()
    {
        float percentOfMaxSpeed = carController.PercentOfMaxSpeed();
        bool isCarMovingForward = carController.CurrentSpeed() >= 0;

        // Quand le véhicule avance, la caméra est devant le véhicule, derrière quand il recule
        Vector3 cameraPositionRelativeToCar = (isCarMovingForward ? 1 : -1) * distanceAhead * percentOfMaxSpeed * transform.up;
        
        transform.position = transform.parent.position + cameraPositionRelativeToCar + offset;
    }
}
