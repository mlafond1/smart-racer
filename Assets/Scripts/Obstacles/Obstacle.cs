using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    //Le comportement de l'obstacle
    public abstract void Comportement(Collider2D collider);
}