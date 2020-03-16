

public class CarStatistics {

    public float maxSpeed{get; private set;}
    public float torqueSpeed{get; private set;}
    public float driftPourcentage{get; private set;}

    public CarStatistics(float maxSpeed, float torqueSpeed, float driftPourcentage){
        this.maxSpeed = maxSpeed;
        this.torqueSpeed = torqueSpeed;
        this.driftPourcentage = driftPourcentage;
    }

}