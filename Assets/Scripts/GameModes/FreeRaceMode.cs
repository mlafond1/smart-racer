using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Peut ne pas être reconnu dans l'IDE, mais le jeu fonctionne... (et Text)

public class FreeRaceMode : TimeTrialMode
{

    List<CarRaceInfo> carInfos;
    Text playerRankPanel;

    protected override void Start(){
        base.Start();
        playerRankPanel = GameObject.Find("RankText").GetComponent<Text>();
        playerRankPanel.enabled = true;
        carInfos = new List<CarRaceInfo>(raceInfos.Values);
        UpdateRanking();
        DisplayRanking();
    }

    protected override void Update(){
        if(gameEnded) return;
        base.Update();
        DisplayRanking();
    }

    protected override void FixedUpdate(){
        if(gameEnded) return;
        base.FixedUpdate();
        UpdateRanking();
    }

    protected void UpdateRanking(){
        carInfos.Sort((first, second) => first.CompareTo(second));
        int rank = 1;
        foreach(CarRaceInfo carInfo in carInfos){
            carInfo.RankingInRace = rank;
            ++rank;
        }
    }

    void DisplayRanking(){
        int playerRanking = raceInfos[playerCar].RankingInRace;
        playerRankPanel.text = string.Format("Rank: {0}", playerRanking);
    }
}
