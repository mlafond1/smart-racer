using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject panelJouer;
    public GameObject panelModes;
    public GameObject panelPiste;

    //Paramètres passés à la scène suivante
    private static string modesJeu;
    private static int pisteCourse;

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
        }
    }

    public void ModesJeu(string mode)
    {
        modesJeu = mode;
        panelModes.SetActive(false);
        panelPiste.SetActive(true);
        Debug.Log(modesJeu);
    }

    public void PisteCourse(int id)
    {
        pisteCourse = id;
        SceneManager.LoadScene(id);
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
