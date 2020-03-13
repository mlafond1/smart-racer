using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Peut ne pas être reconnu dans l'IDE, mais le jeu fonctionne... (et Text)

public class TimeTrialMode : MonoBehaviour
{
    int numberOfLaps = 3;
    float raceTime = 0f;

    protected Collider2D finishLine;
    protected List<Collider2D> checkpoints;
    protected List<Obstacle> obstacles;
    protected CarController playerCar;
    protected List<CarController> cars;
    protected Dictionary<CarController, CarRaceInfo> raceInfos;

    Text timePanel;
    Text countdownPanel;
    Text playerLapsPanel;
    Text speedPanel;

    protected virtual void Start()
    {
        timePanel = GameObject.Find("TimeText").GetComponent<Text>();
        countdownPanel = GameObject.Find("CountdownText").GetComponent<Text>();
        playerLapsPanel = GameObject.Find("LapsText").GetComponent<Text>();
        speedPanel = GameObject.Find("SpeedText").GetComponent<Text>();

        checkpoints = new List<Collider2D>(GameObject.Find("RaceCheckpoints").GetComponentsInChildren<Collider2D>());
        obstacles = new List<Obstacle>(GameObject.Find("piste").GetComponentsInChildren<Obstacle>());
        finishLine = GameObject.Find("FinishLine").GetComponent<Collider2D>();
        // met la ligne d'arrivée à la fin de la liste
        checkpoints.Remove(finishLine);
        checkpoints.Add(finishLine);

        cars = new List<CarController>(GameObject.FindObjectsOfType<CarController>());
        raceInfos = new Dictionary<CarController, CarRaceInfo>(cars.Count * 3);
        foreach (CarController car in cars){
            Collider2D carCollider = car.gameObject.GetComponent<Collider2D>();
            CarRaceInfo info = new CarRaceInfo(carCollider, checkpoints[0]);
            car.SetCheckpoint(checkpoints[checkpoints.Count - 1]);
            raceInfos.Add(car, info);
        }
        playerCar = GameObject.FindObjectOfType<PlayerController>().gameObject.GetComponent<CarController>();

        RefreshLapsPanel();

        StartCoroutine("Countdown");
    }

    protected virtual void Update(){
        DisplayRaceTime();
        DisplayPlayerSpeed();
    }

    protected virtual void FixedUpdate(){
        TrackLapsForCars();
    }

    void TrackLapsForCars(){
        foreach (var item in raceInfos){
            CarController car = item.Key;
            CarRaceInfo info  = item.Value;
            if(info.carCollider.IsTouching(info.NextCheckpoint)){
                ProceedToNextCheckpoint(car, info);
            }
            Debug.DrawLine(car.transform.position, car.checkpoint.transform.position, Color.blue);
        }
    }

    void ProceedToNextCheckpoint(CarController car, CarRaceInfo info){
        car.SetCheckpoint(info.NextCheckpoint);
        ++info.numberOfCheckpoints;
        if(info.NextCheckpoint.Equals(finishLine)){
            ProceedToNextLap(car, info);
        }
        // Il faut que les checkpoints soient dans l'ordre dans l'éditeur!
        info.NextCheckpoint = checkpoints[info.numberOfCheckpoints];
        //TODO refactor CarRaceInfo -- attribut de CarController
    }

    void ProceedToNextLap(CarController car, CarRaceInfo info){
        info.numberOfCheckpoints = 0;
        ++info.Lap;
        if(info.Lap > numberOfLaps){
            //TODO Terminer la course/jeu après le dernier tour
            
            // temporairement...
            car.enabled = false;
            info.FinalRaceTime = raceTime;
            if(car.Equals(playerCar)){
                countdownPanel.text = "Terminé!";
                countdownPanel.enabled = true;
                this.enabled = false; // Arrêter le temps
                ToggleCars(false); // Arrêter les véhicules
            }
        } else {
            RefreshLapsPanel();
        }
    }

    void RefreshLapsPanel(){
        playerLapsPanel.text = string.Format("Laps: {0}/{1}", raceInfos[playerCar].Lap, numberOfLaps);
    }

    void DisplayRaceTime(){
        int minutes = (int) (raceTime/60f);
        float secondsAndMillis = raceTime - ((float)minutes*60f);
        timePanel.text = string.Format("Time: {0:00}:{1:00.000}", minutes, secondsAndMillis);
        raceTime += Time.deltaTime;
    }

    void DisplayPlayerSpeed(){
        speedPanel.text = string.Format("Speed: {0:0.0} us", playerCar.CurrentSpeed());
    }

    private IEnumerator Countdown() {
        ToggleCars(false);
        this.enabled = false; // Désactiver le temps pendant le décompte
        countdownPanel.enabled = true;
        for(int count = 3; count > 0; --count) {
            countdownPanel.text = count.ToString();
            yield return new WaitForSeconds(1);
        }
        StartCoroutine("DisplayGo");
        this.enabled = true;
        ToggleCars(true);
    }

    private IEnumerator DisplayGo(){
        countdownPanel.text = "GO!";
        yield return new WaitForSeconds(0.75f);
        countdownPanel.enabled = false;
    }

    void ToggleCars(bool activate){
        foreach (CarController car in cars){
            car.enabled = activate;
        }
    }

}
