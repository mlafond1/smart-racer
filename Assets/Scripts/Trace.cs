using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Trace : MonoBehaviour
{
    public Color couleurLigne;

    private List<Transform> noeuds = new List<Transform>();

    /**
     * Affichage du tracé dans l'éditeur
     * Seulement lorsque AI_Checkpoints est sélectionné
     */
    void OnDrawGizmosSelected()
    {
        //Couleur de la ligne
        Gizmos.color = couleurLigne;

        Transform[] traceTransform = GetComponentsInChildren<Transform>();
        noeuds = new List<Transform>();

        //Ajout des noeuds présents dans le tracé
        for(int i = 0; i < traceTransform.Length; i++)
        {
            if(traceTransform[i] != transform)
            {
                noeuds.Add(traceTransform[i]);
            }
        }

        //Affichage des lignes entre les noeuds
        for(int i = 0; i < noeuds.Count; i++)
        {
            Vector3 noeudCourant = noeuds[i].position;
            Vector3 noeudAvant = Vector3.zero;            

            if(i > 0) {      
                noeudAvant = noeuds[i - 1].position;
            } else if(i == 0 && noeuds.Count > 1)
            {
                noeudAvant = noeuds[noeuds.Count - 1].position;
            }

            Debug.DrawLine(noeudAvant, noeudCourant, Color.red);
            
            
        }

    }

}
