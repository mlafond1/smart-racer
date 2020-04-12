using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instanciation : MonoBehaviour
{
    public GameObject prefabScene;    
    public GameObject v_Joueur, v_IA1, v_IA2, v_IA3;
    public GameObject prefabMonteCarlo, prefabLeMans;
    private GameObject pos1, pos2, pos3, posJ;
    private GameObject gameManager;    
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
        else
        {
            creationInstances(GM.getGameMode(), GM.getPiste());
        }        
    }

    private void creationInstances(string mode, int piste)
    {
        GameObject prefab = new GameObject();

        if (mode == "ContreLaMontre")
        {
            //Récupération du prefab correspondant à la piste choisie
            if (piste == 1) prefab = prefabMonteCarlo;
            else if (piste == 2) prefab = prefabLeMans;

            //Récupération des positions/rotations pour l'environnement            
            Vector3 positionEnv = prefabScene.transform.position;
            Quaternion rotationEnv = prefabScene.transform.rotation;            
            
            //Instanciation de l'environnement
            Instantiate(prefab, positionEnv, rotationEnv);

            //Désactivation du DamagePanel
            GameObject damagePanel = GameObject.Find("DamagePanel");            
            damagePanel.SetActive(false);
            

            //Récupération des positions de départ
            //Ces positions sont présentes dans les deux prefabs des pistes
            pos1 = GameObject.Find("Pos1");
            pos2 = GameObject.Find("Pos2");
            pos3 = GameObject.Find("Pos3");
            posJ = GameObject.Find("PosJ");

            //Récupération des positions/rotations pour le véhicule du joueur            
            Vector3 positionJ = posJ.transform.position;
            Quaternion rotationJ = posJ.transform.rotation;
            
            //Instanciation du véhicule du joueur
            //L'objet est ajouté à la hiérarchie de prefabScene, en conservant sa position
            GameObject instance = Instantiate(v_Joueur, positionJ, rotationJ);
            instance.transform.SetParent(prefabScene.transform, true);
            
        }
    }

}
