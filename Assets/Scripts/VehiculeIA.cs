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
        carController.horizontalAxis = newSteer;

        //Accélération du véhicule
        carController.Accelerate();
    }

}
