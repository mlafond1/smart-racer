using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instanciation : MonoBehaviour
{
    public GameObject prefabScene;    
    public GameObject v_Joueur;
    public GameObject v_IA1;
    public GameObject v_IA2;
    public GameObject v_IA3;
    public GameObject prefabMonteCarlo;
    public GameObject prefabLeMans;
    public GameObject gameManager;
    private GameManager GM;


    // Start is called before the first frame update
    void Start()
    {
        //Récupération du game manager
        gameManager = GameObject.Find("GameManager");
        GM = gameManager.GetComponent<GameManager>();
        
        Debug.Log(GM.getPiste());
        Debug.Log(GM.getGameMode());

        if (GM.getGameMode() == "Tournoi")
        {

        }
        else if (GM.getGameMode() == "CourseLibre")
        {

        } else if(GM.getGameMode() == "ContreLaMontre")
        {

        } else if(GM.getGameMode() == "Elimination")
        {

        }

    }

}
