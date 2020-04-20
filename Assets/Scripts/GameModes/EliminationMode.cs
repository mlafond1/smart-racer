using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminationMode : FreeRaceMode
{

    int nbOfActiveCars = 0;

    protected override void Start(){
        base.Start();
        nbOfActiveCars = cars.Count;
    }

    protected override void Update(){
        base.Update();
    }

    protected override void FixedUpdate(){
        base.FixedUpdate();
    }

    protected override void ProceedToNextLap(CarController car, CarRaceInfo info){
        base.ProceedToNextLap(car, info);
        if(nbOfActiveCars == 1) return;
        UpdateRanking();
        // Si pas avant dernière on fait rien
        if(info.RankingInRace != nbOfActiveCars -1) return;

        foreach(var item in raceInfos){
            if(item.Value.RankingInRace == nbOfActiveCars){
                Eliminate(item.Key, item.Value);
                break;
            }
        }
    }

    void Eliminate(CarController car, CarRaceInfo info){
        car.enabled = false;
        --nbOfActiveCars;
        if(car.Equals(playerCar)){
            EndGame();
        }
    }
}
