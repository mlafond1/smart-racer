using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    CarController carController;
    [SerializeField]
    Collider2D finishLine = null;//TODO

    void Start(){
        carController = GetComponent<CarController>();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            carController.Aim(GetMousePosition());
            carController.UseItem(0);
        } 
        if(Input.GetKeyDown(KeyCode.Mouse1)){
            carController.Aim(GetMousePosition());
            carController.UseItem(1);
        }
        // TODO Delete after debug
        if (Input.GetKeyDown("r"))
        {
            carController.Respawn();
        }
        if (Input.GetKeyDown("t"))
        {
            carController.Respawn(finishLine);
        }
        // TODO
        Debug.DrawRay(this.transform.position, this.GetComponent<Rigidbody2D>().velocity, Color.blue);
        // TODO
    }

    void FixedUpdate(){
        if(Input.GetButton("Accelerate")) carController.Accelerate();
        else if(Input.GetButton("Break")) carController.Brake();
        carController.Steer(Input.GetAxis("Horizontal"));
    }

    private Vector3 GetMousePosition(){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        return mousePosition;
    }

}
