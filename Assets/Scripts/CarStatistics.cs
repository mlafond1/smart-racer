

public class CarStatistics {

    public float maxSpeed{get; private set;}
    public float torqueSpeed{get; private set;}
    public float driftPercentage{get; private set;}
    public float ejectionRate{get; private set;}

    public CarStatistics(float maxSpeed, float torqueSpeed, float driftPercentage){
        this.maxSpeed = maxSpeed;
        this.torqueSpeed = torqueSpeed;
        this.driftPercentage = driftPercentage;
        this.ejectionRate = 1f;
    }

    public void ApplyDamage(float damage){
        this.ejectionRate += damage/100f;
    }

}