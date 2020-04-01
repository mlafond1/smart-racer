using UnityEngine;
//using UnityEngine.Experimental.Rendering.​Universal;
public class Tunnel : Obstacle
{
    // [SerializeField]
    private GameObject global = null;
    [SerializeField]
    private PlayerController player = null;
    private string globalLightName = "Global Light 2D";

    private void Awake() {
        global = GameObject.Find(globalLightName);
        player = GameObject.FindObjectOfType<PlayerController>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController car = other.gameObject.GetComponent<PlayerController>();
        if(car != null && car == this.player)
        {
            car.transform.Find("Lights").gameObject.SetActive(true);
            car.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.global.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        PlayerController car = other.gameObject.GetComponent<PlayerController>();
        if (car != null && car == this.player)
        {
            car.transform.Find("Lights").gameObject.SetActive(false);
            car.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            this.global.SetActive(true);
        }
    }
}