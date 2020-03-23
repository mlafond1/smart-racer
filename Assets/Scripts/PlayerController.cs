using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    CarController carController;

    // TEMP cycler à travers les items
    Item[] offensiveItems;
    int offensiveItemsIndex = 0;
    Item[] defensiveItems;
    int defensiveItemsIndex = 0;

    void Start(){
        carController = GetComponent<CarController>();
        // TEMP cycler à travers les items
        offensiveItems = new Item[] { 
            new MissileItem(carController),
            new BladeItem(carController), 
            new HarpoonItem(carController)
        };
        defensiveItems = new Item[] { 
            new ShieldItem(carController), 
            new ReflectShieldItem(carController),
            new GhostItem(carController)
        };
        carController.SetItem(0, offensiveItems[0]);
        carController.SetItem(1, defensiveItems[0]);
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
        // TEMP cycler à travers les items
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            offensiveItemsIndex = (offensiveItemsIndex + 1) % offensiveItems.Length;
            carController.SetItem(0, offensiveItems[offensiveItemsIndex]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            defensiveItemsIndex = (defensiveItemsIndex + 1) % defensiveItems.Length;
            carController.SetItem(1, defensiveItems[defensiveItemsIndex]);
        }
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
