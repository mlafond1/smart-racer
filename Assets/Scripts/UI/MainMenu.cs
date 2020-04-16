using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject panelJouer;
    public GameObject panelModes;
    public GameObject panelPiste;
    public GameObject panelTournoi;
    public GameObject panelML;
    public GameObject gameManager;
    private GameManager GM;

    //Paramètres passés à la scène suivante
    private static string modesJeu;
    private static int pisteCourse;

    public void Start()
    {
        GM = gameManager.GetComponent<GameManager>();
    }

    public void Jouer()
    {
        panelJouer.SetActive(false);
        panelModes.SetActive(true);
    }

    public void Retour(GameObject objAfficher) {
        if (objAfficher == panelModes)
        {
            panelModes.SetActive(false);
            panelJouer.SetActive(true);

        } else if (objAfficher == panelPiste)
        {
            panelPiste.SetActive(false);
            panelModes.SetActive(true);       

        } else if (objAfficher == panelTournoi)
        {
            panelTournoi.SetActive(false);
            panelModes.SetActive(true);

        } else if(objAfficher == panelML)
        {
            panelML.SetActive(false);
            panelModes.SetActive(true);

        }
    }

    public void ModesJeu(string mode)
    {
        modesJeu = mode;
        GM.SetGameMode(mode);
        panelModes.SetActive(false);

        //Si le mode sélectionné est Tournoi, il n'y a pas de sélection de piste
        if (mode == "Tournoi")
        {
            panelTournoi.SetActive(true);
        } else if (mode == "ML")
        {
            panelML.SetActive(true);
        }
        else
        {
            panelPiste.SetActive(true);
        }

        //À ENLEVER
        //Debug.Log((string)GM.getGameMode());
    }

    public void PisteCourse(int id)
    {
        pisteCourse = id;
        GM.SetPiste(id);
        SceneManager.LoadScene(1);

        //À ENLEVER
        //Debug.Log(GM.getPiste());
        

    }

    public void CommencerTournoi(){
        List<int> pistesTournoi = new List<int>{1,2};
        GM.TournamentTracks = pistesTournoi;
        GM.TournamentIndex = 0;
        // TODO Scoreboard à zero
        SceneManager.LoadScene(1);
    }

    public void DemoML()
    {
        SceneManager.LoadScene(2);
    }

    public void Quitter()
    {
#if UNITY_EDITOR        
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }   


}
