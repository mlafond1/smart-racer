using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Peut ne pas être reconnu dans l'IDE, mais le jeu fonctionne... (et Text)

public class TimeTrialMode : MonoBehaviour
{

    float raceTime = 0f;
    List<CarController> cars;
    Text timePanel;
    Text countdownPanel;

    void Start()
    {
        timePanel = GameObject.Find("TimeText").GetComponent<Text>();
        countdownPanel = GameObject.Find("CountdownText").GetComponent<Text>();
        cars = new List<CarController>(GameObject.FindObjectsOfType<CarController>());
        
        StartCoroutine("Countdown");
    }

    void Update(){
        DisplayRaceTime();
    }

    void DisplayRaceTime(){
        int minutes = (int) (raceTime/60f);
        float secondsAndMillis = raceTime - ((float)minutes*60f);
        timePanel.text = string.Format("Time: {0:00}:{1:00.000}", minutes, secondsAndMillis);
        raceTime += Time.deltaTime;
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
