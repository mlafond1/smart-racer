

public class CarStatistics {

    public float maxSpeed{get; private set;}
    public float torqueSpeed{get; private set;}
    public float driftPercentage{get; private set;}
    public float power{get; private set;}
    public float attackDamage{get; private set;}
    public float ejectionRate{get; private set;}

    private bool canTakeDamage;

    public CarStatistics(float maxSpeed, float torqueSpeed, float driftPercentage, float power, float attackDamage){
        this.maxSpeed = maxSpeed;
        this.torqueSpeed = torqueSpeed;
        this.driftPercentage = driftPercentage;
        this.power = power;
        this.attackDamage = attackDamage;
        this.ejectionRate = 1f;
        this.canTakeDamage = true;
    }

    public void ApplyDamage(float damage){
        if(!canTakeDamage) return;
        this.ejectionRate += damage/100f;
    }

    public void ToggleDamage(bool canTakeDamage){
        this.canTakeDamage = canTakeDamage;
    }

    public void UpdateOffensiveStats(float powerIncrease, float attackDamageIncrease){
        this.power += powerIncrease;
        this.attackDamage += attackDamageIncrease;
    }

}