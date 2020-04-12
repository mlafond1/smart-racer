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
        if(objAfficher == panelModes)
        {
            panelPiste.SetActive(false);
            panelModes.SetActive(true);

        } else if (objAfficher == panelJouer)
        {
            panelModes.SetActive(false);
            panelJouer.SetActive(true);
        } else if(objAfficher == panelTournoi)
        {
            panelTournoi.SetActive(false);
            panelModes.SetActive(true);
        }
    }

    public void ModesJeu(string mode)
    {
        modesJeu = mode;
        GM.setGameMode(mode);
        panelModes.SetActive(false);

        //Si le mode sélectionné est Tournoi, il n'y a pas de sélection de piste
        if (mode == "Tournoi")
        {
            panelTournoi.SetActive(true);
        }
        else
        {
            panelPiste.SetActive(true);
        }

        //À ENLEVER
        Debug.Log((string)GM.getGameMode());
    }

    //Le bouton COMMENCER du panelTournoi utilise aussi cette fonction
    //et commence toujours un tournoi avec la piste MonteCarlo
    public void PisteCourse(int id)
    {
        pisteCourse = id;
        GM.setPiste(id);
        SceneManager.LoadScene(1);

        //À ENLEVER
        Debug.Log(GM.getPiste());
        

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
