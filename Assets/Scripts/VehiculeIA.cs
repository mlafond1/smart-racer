using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculeIA : MonoBehaviour
{
    public Transform trace;
    public float maxSteerAngle = 45f;    

    private List<Transform> noeuds;
    private int noeudCourant = 0;
    

    private void Start()
    {
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
    }
        
    private void FixedUpdate()
    {
        Conduire();
    }

    private void Conduire()
    {
        //Acquisition du script de contrôle du véhicule
        CarController carController = GetComponent<CarController>();

        //Calcul de l'angle du prochain noeud du tracé en fonction de la position du véhicule
        Vector3 relativeVector = transform.InverseTransformPoint(noeuds[noeudCourant].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;

        //Application de l'angle au steer du carController
        //carController.horizontalAxis = newSteer;
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

}
