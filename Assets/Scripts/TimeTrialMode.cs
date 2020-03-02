using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Peut ne pas être reconnu dans l'IDE, mais le jeu fonctionne... (et Text)

public class TimeTrialMode : MonoBehaviour
{
    int numberOfLaps = 3;
    float raceTime = 0f;

    Collider2D finishLine;
    List<Collider2D> checkpoints;
    
    CarController playerCar;
    List<CarController> cars;
    Dictionary<CarController, CarRaceInfo> raceInfos;

    Text timePanel;
    Text countdownPanel;
    Text playerLapsPanel;
    Text speedPanel;

    void Start()
    {
        timePanel = GameObject.Find("TimeText").GetComponent<Text>();
        countdownPanel = GameObject.Find("CountdownText").GetComponent<Text>();
        playerLapsPanel = GameObject.Find("LapsText").GetComponent<Text>();
        speedPanel = GameObject.Find("SpeedText").GetComponent<Text>();

        checkpoints = new List<Collider2D>(GameObject.Find("RaceCheckpoints").GetComponentsInChildren<Collider2D>());
        finishLine = GameObject.Find("FinishLine").GetComponent<Collider2D>();
        // met la ligne d'arrivée à la fin de la liste
        checkpoints.Remove(finishLine);
        checkpoints.Add(finishLine);

        cars = new List<CarController>(GameObject.FindObjectsOfType<CarController>());
        raceInfos = new Dictionary<CarController, CarRaceInfo>(cars.Count * 3);
        foreach (CarController car in cars){
            Collider2D carCollider = car.gameObject.GetComponent<Collider2D>();
            CarRaceInfo info = new CarRaceInfo(carCollider, checkpoints[0]);
            raceInfos.Add(car, info);
        }
        playerCar = GameObject.FindObjectOfType<PlayerController>().gameObject.GetComponent<CarController>();

        RefreshLapsPanel();

        StartCoroutine("Countdown");
    }

    void Update(){
        DisplayRaceTime();
        DisplayPlayerSpeed();
    }

    void FixedUpdate(){
        TrackLapsForCars();
    }

    void TrackLapsForCars(){
        foreach (var item in raceInfos){
            CarController car = item.Key;
            CarRaceInfo info  = item.Value;
            if(info.carCollider.IsTouching(info.NextCheckpoint)){
                ProceedToNextCheckpoint(car, info);
            }
        }
    }

    void ProceedToNextCheckpoint(CarController car, CarRaceInfo info){
        ++info.numberOfCheckpoints;
        if(info.NextCheckpoint.Equals(finishLine)){
            ProceedToNextLap(car, info);
        }
        // Il faut que les checkpoints soient dans l'ordre dans l'éditeur!
        info.NextCheckpoint = checkpoints[info.numberOfCheckpoints];
    }

    void ProceedToNextLap(CarController car, CarRaceInfo info){
        info.numberOfCheckpoints = 0;
        ++info.Lap;
        if(info.Lap > numberOfLaps){
            //TODO Terminer la course/jeu après le dernier tour
            
            // temporairement...
            countdownPanel.text = "Terminé!";
            countdownPanel.enabled = true;
            car.enabled = false;
            if(car.Equals(playerCar)){
                car.gameObject.GetComponent<PlayerController>().enabled = false;
                this.enabled = false; // Arrêter le temps
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
            PlayerController player = car.gameObject.GetComponent<PlayerController>();
            if(player != null) player.enabled = activate;
        }
    }

}
