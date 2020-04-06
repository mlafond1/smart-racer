using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;
public class CarDriveAgent : Agent
{
    [SerializeField]
    private Track track = null;

    [SerializeField]
    private GameObject finishLine = null;
    private Collider2D trackCollider;

    private CarController controller;
    private float maxTimeOffTrack = 2f;
    private int maxNumTimesOffTrack = 5*2;
    private Coroutine offTrack = null;
    public Transform trace;
    public LinkedList<Transform> rewardGates;
    public LinkedListNode<Transform> currentGate;

    public override void Initialize()
    {
        if (track == null) track = GameObject.FindObjectOfType<Track>();
        trackCollider = track.GetComponent<EdgeCollider2D>();
        controller = GetComponent<CarController>();

        Transform[] traceTransform = trace.GetComponentsInChildren<Transform>();
        rewardGates = new LinkedList<Transform>();

        //Ajout des noeuds présents dans le tracé
        for (int i = 0; i < traceTransform.Length; i++)
        {
            if (traceTransform[i] != trace.transform)
            {
                rewardGates.AddLast(traceTransform[i]);
            }
        }
        currentGate = rewardGates.First;
    }

    public override void OnEpisodeBegin()
    {
        controller.SetRespawnpoint(finishLine.GetComponent<Collider2D>());
        controller.Respawn();
        currentGate = rewardGates.First;
        controller.offTrackCounter = 0;
        if (offTrack != null){
            controller.StopCoroutine(offTrack);
            offTrack = null;
            // Debug.Log("Stop Chrono Begin");
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((this.transform.position - currentGate.Value.position).sqrMagnitude);
        sensor.AddObservation(currentGate.Value.position.normalized);
        // Position of the next gate
        sensor.AddObservation(currentGate.Next == null? currentGate.List.First.Value.position.normalized: currentGate.Next.Value.position.normalized);
        sensor.AddObservation(controller.GetComponent<Rigidbody2D>().velocity.normalized);
        sensor.AddObservation(controller.GetComponent<Transform>().up.normalized);
        sensor.AddObservation(controller.GetComponent<Transform>().position.normalized);
    }

    public override void OnActionReceived(float[] vectorAction)
    {

        // Save distance from gate before moving
        float distance = (this.transform.position - currentGate.Value.position).sqrMagnitude;
        if (vectorAction[0] > 0) controller.Accelerate();
        else if (vectorAction[0] < 0) controller.Brake();
        // Debug.Log(vectorAction[0]);
        Debug.Log(vectorAction[1]);
        controller.Steer(vectorAction[1]);

        if(controller.offTrackCounter >= maxNumTimesOffTrack){
            AddReward(-1f);
            EndEpisode();
        }

        // If new position is closer to rewardgate
        // if ((this.transform.position - currentGate.Value.position).sqrMagnitude < distance)
        // {
        //     AddReward(-0.01f);
        // }
        // else
        // {
        //     AddReward(-0.015f);
        // }
        Debug.DrawLine(controller.transform.position, currentGate.Value.position, Color.yellow);
        // If touched rewardGate -> reward and move to next gate
        if (controller.GetComponent<Collider2D>().IsTouching(currentGate.Value.GetComponent<Collider2D>()))
        {
            // If gate == finish line more rewards
            if (currentGate.Equals(currentGate.List.First)) AddReward(1f);
            else AddReward(.5f);
            // After touching proceed to next gate
            currentGate = currentGate.Equals(currentGate.List.Last) ? currentGate.List.First : currentGate.Next;
        }
        // Small reward if staying on track
        if (controller.State.GetType() == typeof(OnTrackState) ||
            controller.GetComponent<Collider2D>().IsTouching(trackCollider))
        {
            AddReward(0.001f);
            if(offTrack != null){
                controller.StopCoroutine(offTrack);
                offTrack = null;
                // Debug.Log("Stop Chrono On track");
            }
        }
        else if (controller.State.GetType() == typeof(OffTrackState))
        {
            AddReward(-0.0015f);
            if (offTrack == null){
                // Debug.Log("Start Chrono");
                offTrack = controller.StartCoroutine(this.TimeOffTrack());// controller.StopCoroutine(offTrack);
            }
        }
        //^^^^
        // if (controller.GetComponent<Collider2D>().IsTouching(trackCollider)) AddReward(0.001f);
        // else AddReward(-0.001f);

        // Punishment for taking time
        if (maxStep > 0) AddReward(-1f / maxStep);
    }

    public override float[] Heuristic()
    {
        float[] actions = new float[2];
        if (Input.GetButton("Break")) actions[0] = -1;
        else if (Input.GetButton("Accelerate")) actions[0] = 1;

        actions[1] = Input.GetAxis("Horizontal");

        return actions;
    }

    // private void FixedUpdate()
    // {
    //     if (StepCount % 5 == 0)
    //     {
    //         RequestDecision();
    //     }
    //     else
    //     {
    //         RequestAction();
    //     }
    // }

    private IEnumerator TimeOffTrack()
    {
        float currentTime = 0;
        while (currentTime < maxTimeOffTrack)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        if (currentTime >= maxTimeOffTrack &&
            controller.State.GetType() == typeof(OffTrackState))
        {
            EndEpisode();
        }
    }
}