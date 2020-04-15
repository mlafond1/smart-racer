using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TournamentMode : FreeRaceMode
{

    private enum NextOperation{
        BaseDisplay, CumulativeDisplay, NextTrack, None
    }
    private NextOperation next;

    private GameManager manager;

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

    public void DisplayResult(){

    }

    public void DisplayCumulativeResult(){

    }

    public void GoToNextTrack(){
        manager.TournamentIndex++;
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

}
