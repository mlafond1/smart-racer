using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    //Le comportement de l'obstacle
    public virtual void Comportement(Collider2D collider){}
}