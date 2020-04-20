using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Peut ne pas être reconnu dans l'IDE, mais le jeu fonctionne... (et Text)

public class FreeRaceMode : TimeTrialMode
{

    protected List<CarRaceInfo> carInfos;
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
        base.Update();
        if(gameEnded) return;
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

    protected override void DisplayResult(){
        string scoreBoardText = "";
        // Ordonné selon ranking
        foreach(CarRaceInfo info in carInfos){
            scoreBoardText += info.FormatInfo() + "\n";
        }
        countdownPanel.text = "Score";
        scoreBoardPanel.enabled = true;
        scoreBoardPanel.text = scoreBoardText;
    }
}
