using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Obstacle
{
    // Rigidbodies of wall children (individual bricks)
    private List<Brick> bricks;
    [SerializeField]
    private float resetDelay = 10f; // seconds

    // Start is called before the first frame update
    private void Awake()
    {
        bricks = new List<Brick>();
        FilterBricks();
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
    }

    public void FilterBricks()
    {
        // Get all bricks in game
        Brick[] tmp = GameObject.FindObjectsOfType<Brick>();
        foreach (Brick brick in tmp)
        {
            // Select only bricks from this wall
            if (brick.transform.parent.name == this.name)
            {
                bricks.Add(brick);
            }
        }
    }
}
