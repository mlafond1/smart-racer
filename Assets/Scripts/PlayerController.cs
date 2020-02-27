using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    CarController carController;

    void Start(){
        carController = GetComponent<CarController>();
    }

    void FixedUpdate(){
        if(Input.GetButton("Accelerate")) carController.Accelerate();
        else if(Input.GetButton("Break")) carController.Brake();
        carController.Steer(Input.GetAxis("Horizontal"));
    }
}
