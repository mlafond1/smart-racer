using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRaceInfo {

    public CarRaceInfo(Collider2D carCollider, Collider2D NextCheckpoint){
        Lap = 1;
        numberOfCheckpoints = 0;
        this.carCollider = carCollider;
        this.NextCheckpoint = NextCheckpoint;
    }

    public int Lap{get;set;}
    public int numberOfCheckpoints;
    public Collider2D carCollider{get;set;}
    public Collider2D NextCheckpoint{get;set;}

}