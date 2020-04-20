using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TournamentMode : FreeRaceMode
{

    private enum NextOperation{
        BaseDisplay, CumulativeDisplay, NextTrack, None
    }
    private NextOperation next;
    private GameManager manager;
    private Dictionary<CarController, int> scores = new Dictionary<CarController, int>();

    protected override void Start(){
        base.Start();
        next = NextOperation.BaseDisplay;
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    protected override void Update(){
        if(!gameEnded){
            base.Update();
        }
        else if(Input.GetKeyDown(KeyCode.Space)){
            switch(next){
                case NextOperation.BaseDisplay:
                    DisplayResult();
                    next = NextOperation.CumulativeDisplay;
                    break;
                case NextOperation.CumulativeDisplay:
                    DisplayCumulativeResult();
                    next = NextOperation.NextTrack;
                    break;
                case NextOperation.NextTrack:
                    GoToNextTrack();
                    next = NextOperation.None;
                    break;
                case NextOperation.None:
                default:
                    Debug.Log(next.ToString());
                    break;
            }
        }
    }

    protected override void FixedUpdate(){
        if(gameEnded) return;
        base.FixedUpdate();
    }

    protected override void DisplayResult(){
        string scoreBoardText = "";
        foreach(CarRaceInfo info in carInfos){
            scoreBoardText += info.FormatInfo() + "\t+" + CalculateScoreForCar(info) + " pts\n";
        }
        countdownPanel.text = "Score";
        scoreBoardPanel.enabled = true;
        scoreBoardPanel.text = scoreBoardText;
    }

    private void DisplayCumulativeResult(){
        foreach(var car in cars){
            if(!manager.TournamentScores.ContainsKey(car.name)){
                manager.TournamentScores.Add(car.name, 0);
            }
            int cumulativeScore  = CalculateScoreForCar(raceInfos[car]) + manager.TournamentScores[car.name];
            scores.Add(car, cumulativeScore);
        }
        cars.Sort((first,second) => -scores[first].CompareTo(scores[second])); // Du plus grand au plus petit
        string scoreBoardText = "";
        int rank = 1;
        foreach(var car in cars){
            bool needAdditionalTab = car.name.Length <= 4;
            scoreBoardText += string.Format("#{0} {1}:\t\t\t\t\t\t{2} {3} pts\n", rank++, car.name, needAdditionalTab ? "\t\t" : "", scores[car]);
        }
        countdownPanel.text = "Cumulatif";
        scoreBoardPanel.enabled = true;
        scoreBoardPanel.text = scoreBoardText;
    }

    public void GoToNextTrack(){
        manager.TournamentIndex++;
        foreach(var item in scores){
            CarController car = item.Key;
            int score = item.Value;
            manager.TournamentScores[car.name] = score;
        }
        if(manager.TournamentIndex >= manager.TournamentTracks.Count){
            manager.TournamentIndex = 0;
            manager.TournamentTracks = new List<int>();
            SceneManager.LoadScene("MainMenu");
        }
        else {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    private int CalculateScoreForCar(CarRaceInfo info){
        return 5 - info.RankingInRace;
    }

}
