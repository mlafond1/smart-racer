using System.Collections.Generic;
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
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((this.transform.position - currentGate.Value.position).sqrMagnitude);
        sensor.AddObservation(controller.GetComponent<Rigidbody2D>().velocity);
        sensor.AddObservation(controller.GetComponent<Transform>().up);
        sensor.AddObservation(controller.GetComponent<Transform>().position);
    }

    public override void OnActionReceived(float[] vectorAction)
    {

        // Save distance from gate before moving
        float distance = (this.transform.position - currentGate.Value.position).sqrMagnitude;
        if (vectorAction[0] > 0) controller.Accelerate();
        else if (vectorAction[0] < 0) controller.Brake();
        controller.Steer(vectorAction[1]);

        // If new position is closer to rewardgate
        if ((this.transform.position - currentGate.Value.position).sqrMagnitude < distance)
        {
            AddReward(0.1f);
        }
        // If touched rewardGate -> reward and move to next gate
        Debug.Log(currentGate.Value.GetComponent<Collider2D>());
        if (controller.GetComponent<Collider2D>().IsTouching(currentGate.Value.GetComponent<Collider2D>()))
        {
            // If gate == finish line more rewards
            if (currentGate.Equals(currentGate.List.First)) AddReward(10f);
            else AddReward(5f);
            // After touching proceed to next gate
            currentGate = currentGate.Equals(currentGate.List.Last) ? currentGate.List.First : currentGate.Next;
        }
        // Small reward if staying on track
        if (controller.State.GetType() == typeof(OnTrackState)) AddReward(0.001f);
        else if (controller.State.GetType() == typeof(OffTrackState))
        {
            AddReward(-0.1f);
            EndEpisode();
        }
        //^^^^
        if (controller.GetComponent<Collider2D>().IsTouching(trackCollider)) AddReward(0.001f);
        else AddReward(-0.001f);

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

    private void FixedUpdate()
    {
        if (StepCount % 5 == 0)
        {
            RequestDecision();
        }
        else
        {
            RequestAction();
        }
    }

}