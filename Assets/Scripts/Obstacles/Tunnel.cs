using UnityEngine;
//using UnityEngine.Experimental.Rendering.​Universal;
public class Tunnel : Obstacle
{
    [SerializeField]
    private GameObject global = null;
    [SerializeField]
    private string playerName = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CarController car = other.gameObject.GetComponent<CarController>();
        if(car != null && car.name == this.playerName)
        {
            car.transform.Find("Lights").gameObject.SetActive(true);
            car.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.global.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        CarController car = other.gameObject.GetComponent<CarController>();
        if (car != null && car.name == this.playerName)
        {
            car.transform.Find("Lights").gameObject.SetActive(false);
            car.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            this.global.SetActive(true);
            Debug.Log(car.State.GetType() + ":TunnelExit");
        }
    }
    //public override void Comportement(Collider2D collider){}

}