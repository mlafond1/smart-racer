using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Obstacle
{
    // Rigidbodies of wall children (individual bricks)
    private List<Brick> bricks;
    [SerializeField]
    private float resetDelay = 10f; // seconds
    [SerializeField]
    private float bricksMass = 1f;
    [SerializeField]
    private bool dynamic = false;
    private float zPosition = -2.585f;

    // Start is called before the first frame update
    private void Awake()
    {
        Vector3 tmp = transform.position;
        tmp.z = zPosition;
        this.transform.position = tmp;
        bricks = new List<Brick>();
        FilterBricks();
        SetBricksMass(bricksMass);
        this.GetComponentInChildren<BoxCollider2D>().enabled = !dynamic;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If collider is not child
        if (!other.gameObject.transform.IsChildOf(this.transform))
        {
            this.Comportement(other);
        }
    }
    public void Comportement(Collider collider)
    {
        // Do not register collisions until reset/rebuilt
        this.GetComponent<BoxCollider>().enabled = false;
        foreach (Brick brick in bricks)
        {
            // Unfreeze everything
            brick.Freeze(false);
        }
        StartCoroutine(ResetWall());
    }

    public IEnumerator ResetWall()
    {
        yield return new WaitForSeconds(resetDelay);
        foreach (Brick brick in bricks)
        {
            brick.ResetOriginalState();
        }
        this.GetComponent<BoxCollider>().enabled = true;
        this.GetComponentInChildren<BoxCollider2D>().enabled = !dynamic;
    }

    public void SetBricksMass(float newMass){
        foreach (Brick brick in bricks)
        {
            brick.GetComponent<Rigidbody>().mass = newMass;
        }
    }
    public void FilterBricks()
    {
        // Get all bricks in game
        Brick[] tmp = GameObject.FindObjectsOfType<Brick>();
        foreach (Brick brick in tmp)
        {
            // Select only bricks from this wall
            if (brick.transform.IsChildOf(this.transform))
            {
                bricks.Add(brick);
            }
        }
    }
}
