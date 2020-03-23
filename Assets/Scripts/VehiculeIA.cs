using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculeIA : MonoBehaviour
{
    public Transform trace;
    public float maxSteerAngle = 45f;    

    private List<Transform> noeuds;
    private int noeudCourant = 0;
    private CarController carController;

    private void Start()
    {
        //Acquisition du script de contrôle du véhicule
        carController = GetComponent<CarController>();

        Transform[] traceTransform = trace.GetComponentsInChildren<Transform>();
        noeuds = new List<Transform>();

        //Ajout des noeuds présents dans le tracé
        for (int i = 0; i < traceTransform.Length; i++)
        {
            if (traceTransform[i] != trace.transform)
            {
                noeuds.Add(traceTransform[i]);
            }
        }
        // TEMP items de départ
        carController.SetItem(0, new BladeItem(carController));
        carController.SetItem(1, new ReflectShieldItem(carController));
    }
        
    private void FixedUpdate()
    {
        Conduire();
        UtiliserObjets();
    }

    private void UtiliserObjets(){
        Item itemOffensif = carController.GetItem(0);
        Item itemDefensif = carController.GetItem(1);
        if(itemOffensif != null && itemOffensif.isReady){
            // Temp vise vers joueur
            Vector3 playerPosition = GameObject.FindObjectOfType<PlayerController>().transform.position;
            carController.Aim(playerPosition);
            carController.UseItem(0);
        }
        if(itemDefensif != null && itemDefensif.isReady){
            List<ItemEffect> objetsActifs = new List<ItemEffect>(GameObject.FindObjectsOfType<ItemEffect>());
            foreach(ItemEffect effect in objetsActifs){
                // Si l'effet est d'un autre propriétaire
                if(!carController.Equals(effect.GetOwner())){
                    float distance = (effect.transform.position - transform.position).magnitude;
                    if(distance < 5) {
                        carController.UseItem(1);
                        break;
                    }
                }
            }
        }
    }

    private void Conduire()
    {
        //Calcul de l'angle du prochain noeud du tracé en fonction de la position du véhicule
        Vector3 relativeVector = transform.InverseTransformPoint(noeuds[noeudCourant].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        Debug.DrawLine(carController.transform.position, noeuds[noeudCourant].position, Color.black);
        //Ajustement du virage
        newSteer = AjusterVirage(newSteer);

        //Application de l'angle au steer du carController
        carController.Steer(newSteer);

        //Accélération du véhicule
        carController.Accelerate();

        //Aller au prochain noeud
        Collider2D carCollider = carController.gameObject.GetComponent<Collider2D>();
        Collider2D noeudCourantCollider = noeuds[noeudCourant].gameObject.GetComponent<Collider2D>();
        if(carCollider.IsTouching(noeudCourantCollider)){
            noeudCourant = (noeudCourant+1) % noeuds.Count;
        }
    }

    private float AjusterVirage(float newSteer)
    {
        if(InRange(newSteer, -.1f, .1f)) newSteer = 0;
        else if(InRange(newSteer, -.5f, .5f)) newSteer = .05f * Mathf.Sign(newSteer);
        else if(InRange(newSteer, -1f, 1f)) newSteer = .125f * Mathf.Sign(newSteer);
        else if(InRange(newSteer, -1.5f, 1.5f)) newSteer = .25f * Mathf.Sign(newSteer);
        else if(InRange(newSteer, -2f, 2f)) newSteer = .5f * Mathf.Sign(newSteer);

        return newSteer;
    }

    //Utilitaire
    private bool InRange(float value, float min, float max){
        return min <= value && value <= max;
    }
}
