using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider){
        CarController car = collider.gameObject.GetComponent<CarController>();
        if(car != null){
            car.SetOnTrack();
        } 
    }

    void OnTriggerExit2D(Collider2D collider){
        CarController car = collider.gameObject.GetComponent<CarController>();
        if(car != null){
            Rigidbody2D carRigidbody =  car.gameObject.GetComponent<Rigidbody2D>();
            carRigidbody.AddForce(-car.gameObject.transform.up * car.CurrentSpeed());
            car.SetOffTrack();
        } 
    }
}
