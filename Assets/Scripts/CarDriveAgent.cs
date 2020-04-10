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
    private int maxNumTimesOffTrack = 5;
    private int offTrackCounter = 0;
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
        // controller.offTrackCounter = 0;
        offTrackCounter = 0;
        if (offTrack != null)
        {
            controller.StopCoroutine(offTrack);
            offTrack = null;
            // Debug.Log("Stop Chrono Begin");
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((currentGate.Value.position - this.transform.position));
        sensor.AddObservation(currentGate.Value.position);
        // Position of the next gate
        sensor.AddObservation(currentGate.Next == null ? currentGate.List.First.Value.position : currentGate.Next.Value.position);
        sensor.AddObservation(GetComponent<Rigidbody2D>().velocity);
        // Debug.DrawRay(this.transform.position, currentGate.Value.position - this.transform.position, Color.black);
        // Debug.DrawRay(this.transform.position, GetComponent<Rigidbody2D>().velocity, Color.blue);
        // Debug.DrawRay(this.transform.position, transform.up, Color.magenta);
        sensor.AddObservation(this.transform.up);
        sensor.AddObservation(this.transform.position);
    }

    public override void OnActionReceived(float[] vectorAction)
    {

        // Save distance from gate before moving
        float distance = (currentGate.Value.position - this.transform.position).sqrMagnitude;
        if (vectorAction[0] > 0) controller.Accelerate();
        else if (vectorAction[0] < 0) controller.Brake();
        // Debug.Log(vectorAction[0]);
        // Debug.Log(vectorAction[1]);
        controller.Steer(vectorAction[1]);

        // if (controller.offTrackCounter >= maxNumTimesOffTrack)
        // if(offTrackCounter >= maxNumTimesOffTrack)
        // {
        //     AddReward(-1f);
        //     EndEpisode();
        // }
        // Debug.Log(NormalizeAngleDir(
        //         Vector3.Angle(
        //             (currentGate.Value.position - this.transform.position).normalized, GetComponent<Rigidbody2D>().velocity.normalized)));
        // float angle = NormalizeAngleDir(Vector3.Angle(currentGate.Value.position - this.transform.position, GetComponent<Rigidbody2D>().velocity));
        // AddReward(angle * (GetComponent<Rigidbody2D>().velocity)));

        // If new position is closer to rewardgate
        // if ((this.transform.position - currentGate.Value.position).sqrMagnitude < distance)
        // {
        //     AddReward(-0.01f);
        // }
        // else
        // {
        //     AddReward(-0.015f);
        // }
        Debug.DrawLine(this.transform.position, currentGate.Value.position, Color.yellow);
        // If touched rewardGate -> reward and move to next gate
        if (GetComponent<Collider2D>().IsTouching(currentGate.Value.GetComponent<Collider2D>()))
        {
            // If gate == finish line more rewards
            if (currentGate.Equals(currentGate.List.First)) AddReward(0.5f);
            else AddReward(0.1f);
            // After touching proceed to next gate
            currentGate = currentGate.Equals(currentGate.List.Last) ? currentGate.List.First : currentGate.Next;
        }
        // Small reward if staying on track
        if (controller.State.GetType() == typeof(OnTrackState) ||
            GetComponent<Collider2D>().IsTouching(trackCollider))
        {
            AddReward(0.0001f);
            if (offTrack != null)
            {
                controller.StopCoroutine(offTrack);
                offTrack = null;
                // Debug.Log("Stop Chrono On track");
            }
        }
        else if (controller.State.GetType() == typeof(OffTrackState))
        {
            //AddReward(-0.015f);
            if (offTrack == null)
            {
                // Debug.Log("Start Chrono");
                offTrack = controller.StartCoroutine(this.TimeOffTrack());// controller.StopCoroutine(offTrack);
                // offTrackCounter++;
                // Debug.Log(offTrackCounter);
                if (++offTrackCounter >= maxNumTimesOffTrack)
                {
                    AddReward(-0.2f);
                    EndEpisode();
                }
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

    private float NormalizeAngleDir(float angle)
    {
        // Rotate 90 to normalize
        angle = (angle + 90f) % 360f;
        // If opposite of 90 degrees then -1
        float direction = (angle < 180 && angle >= 0) ? 1 : -1;
        // Turn range to 180 - 0
        angle -= (angle > 180f) ? 180f : 0f;
        // Set normalize formula
        float maximum = 90f;
        float minimum = (angle > maximum) ? 180f : 0f;
        // Normalize angle
        return ((angle - minimum) / (maximum - minimum)) * direction;
        // angle = ((angle - minimum) / (maximum - minimum)) * direction;
        // return angle;
    }

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