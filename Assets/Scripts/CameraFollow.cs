using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    float distanceAhead = 5;

    private CarController carController;

    // Start is called before the first frame update
    void Start()
    {
        carController = GetComponentInParent<CarController>();
    }

    void FixedUpdate()
    {
        float percentOfMaxSpeed = carController.PercentOfMaxSpeed();
        bool isCarMovingForward = carController.CurrentSpeed() >= 0;

        // Quand le véhicule avance, la caméra est devant le véhicule, derrière quand il recule
        Vector3 cameraPositionRelativeToCar = (isCarMovingForward ? 1 : -1) * distanceAhead * percentOfMaxSpeed * transform.up;
        Vector3 offset = new Vector3(0,0, -10); // La caméra doit être au dessus du véhicule pour le voir

        transform.position = transform.parent.position + cameraPositionRelativeToCar + offset;
    }
}
