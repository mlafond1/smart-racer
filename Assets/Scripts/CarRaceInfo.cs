using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRaceInfo {

    public CarRaceInfo(Collider2D carCollider, Collider2D NextCheckpoint){
        Lap = 1;
        numberOfCheckpoints = 0;
        this.carCollider = carCollider;
        this.NextCheckpoint = NextCheckpoint;
        this.FinalRaceTime = float.PositiveInfinity;
        this.RankingInRace = 0;
    }

    public float FinalRaceTime{get;set;}
    public int RankingInRace{get;set;}
    public int Lap{get;set;}
    public int numberOfCheckpoints;
    public Collider2D carCollider{get;set;}
    public Collider2D NextCheckpoint{get;set;}

    public int CompareTo(CarRaceInfo other){
        if(this.FinalRaceTime != other.FinalRaceTime) 
            return this.FinalRaceTime.CompareTo(other.FinalRaceTime);
        if(this.Lap != other.Lap) 
            return -this.Lap.CompareTo(other.Lap);
        if(this.numberOfCheckpoints != other.numberOfCheckpoints)
            return -this.numberOfCheckpoints.CompareTo(other.numberOfCheckpoints);
        // Ils sont rendu au même checkpoint au même tour
        float thisDistance =  this.carCollider.Distance(this.NextCheckpoint).distance;
        float otherDistance = other.carCollider.Distance(other.NextCheckpoint).distance;
        return thisDistance.CompareTo(otherDistance);
    }

    public string FormatInfo(){
        bool needAdditionalTab = carCollider.name.Length <= 4;
        if(FinalRaceTime == float.PositiveInfinity){
            return string.Format("#{0} {1}:\t{2}--:--.---\t\t", RankingInRace, carCollider.name, needAdditionalTab ? "\t\t" : "");
        }
        int minutes = (int) (FinalRaceTime/60f);
        float secondsAndMillis = FinalRaceTime - ((float)minutes*60f);
        return string.Format("#{0} {1}:\t{2}{3:00}:{4:00.000}", RankingInRace, carCollider.name, needAdditionalTab ? "\t\t" : "", minutes, secondsAndMillis);
    }

}