using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instanciation : MonoBehaviour
{
    public GameObject prefabScene;    
    public GameObject v_Joueur, v_IA1, v_IA2, v_IA3;
    public GameObject prefabMonteCarlo, prefabLeMans;
    public GameObject TimeTrial, FreeRace, Elimination;

    //Utilisé par les véhicules IA (script VehiculeIA)
    public GameObject AICheckpoints;   

    private GameObject pos1, pos2, pos3, posJ;
    private GameObject gameManager;    
    private GameManager GM;

    


    // Start is called before the first frame update
    void Start()
    {
        //Récupération du game manager
        gameManager = GameObject.Find("GameManager");
        GM = gameManager.GetComponent<GameManager>();

        Debug.Log(GM.GetPiste());
        Debug.Log(GM.GetGameMode());

        if (GM.GetGameMode() == "Tournoi")
        {

        }
        else
        {
            CreationInstances(GM.GetGameMode(), GM.GetPiste());
        }        
    }

    private void CreationInstances(string mode, int piste)
    {


        if (mode == "ContreLaMontre")
        {
            InstancierEnvironnement(mode, piste);
            
            //Récupération des positions de départ
            //Ces positions sont présentes dans les deux prefabs des pistes
            RecupererPosition();           

            //Désactivation du DamagePanel
            GameObject damagePanel = GameObject.Find("DamagePanel");            
            damagePanel.SetActive(false);

            //Instanciation du véhicule du joueur
            InstancierVehicule(posJ, v_Joueur, "joueur", "Joueur");

            //Activation du script TimeTrialMode
            TimeTrial.SetActive(true);            
        } else if(mode == "CourseLibre" || mode == "Elimination")
        {
            //Création de l'environnement
            InstancierEnvironnement(mode, piste);

            //Récupération du transform déterminé par Trace
            AICheckpoints = GameObject.Find("AI Checkpoints");

            //Création des véhicules
            RecupererPosition();
                //Véhicule du joueur
            InstancierVehicule(posJ, v_Joueur, "joueur", "Joueur");
                //Véhicules IA 1 à 3
            InstancierVehicule(pos1, v_IA1, "IA", "IA1");
            InstancierVehicule(pos2, v_IA2, "IA", "IA2");
            InstancierVehicule(pos3, v_IA3, "IA", "IA3");

            if (mode == "CourseLibre")
            {
                FreeRace.SetActive(true);
            } else
            {
                Elimination.SetActive(true);
                //Désactivation du DamagePanel
                GameObject damagePanel = GameObject.Find("DamagePanel");
                damagePanel.SetActive(false);
            }
            
        }

    }

    private void InstancierEnvironnement(string mode, int id)
    {
        GameObject prefab = new GameObject();

        //Récupération du prefab correspondant à la piste choisie
        if (id == 1) prefab = prefabMonteCarlo;
        else if (id == 2) prefab = prefabLeMans;

        //Récupération des positions/rotations pour l'environnement            
        Vector3 positionEnv = prefabScene.transform.position;
        Quaternion rotationEnv = prefabScene.transform.rotation;

        //Instanciation de l'environnement
        Instantiate(prefab, positionEnv, rotationEnv);
    }

    private void InstancierVehicule(GameObject pos, GameObject prefab, string type, string name)
    {        
        //Récupération des positions/rotations pour le véhicule du joueur            
        Vector3 positionJ = pos.transform.position;
        Quaternion rotationJ = pos.transform.rotation;
                //Instanciation du véhicule
        //L'objet est ajouté à la hiérarchie de prefabScene, en conservant sa position
        GameObject instance = Instantiate(prefab, positionJ, rotationJ);
        instance.name = name;
        if (type == "IA")
        {
            //Ajout du Transform Trace au script du véhicule
            instance.GetComponent<VehiculeIA>().trace = AICheckpoints.GetComponent<Trace>().transform;
        }
    }

    private void RecupererPosition()
    {
        pos1 = GameObject.Find("Pos1");
        pos2 = GameObject.Find("Pos2");
        pos3 = GameObject.Find("Pos3");
        posJ = GameObject.Find("PosJ");
    }
}
