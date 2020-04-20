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
    Matrix4x4 m_TargetDirMatrix;

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
        for (int i = 0; i < 2; i++)
        {
            currentGate = currentGate.Next;
        }
    }

    public override void OnEpisodeBegin()
    {
        currentGate = rewardGates.First;
        for (int i = 0; i < 2; i++)
        {
            currentGate = currentGate.Next;
        }
        // controller.SetRespawnpoint(finishLine.GetComponent<Collider2D>());
        controller.SetRespawnpoint(currentGate.Previous.Value.GetComponent<Collider2D>());
        controller.Respawn();
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
        // sensor.AddObservation((currentGate.Value.position - this.transform.position));
        // Vector2 posCurrentGateV2 = new Vector2(currentGate.Value.position.x, currentGate.Value.position.y);
        // sensor.AddObservation(posCurrentGateV2 - GetComponent<Rigidbody2D>().velocity);
        // Position of the next gate
        // sensor.AddObservation(currentGate.Next == null ? currentGate.List.First.Value.position : currentGate.Next.Value.position);
        // Debug.DrawRay(this.transform.position, currentGate.Value.position - this.transform.position, Color.black);
        // Debug.DrawRay(this.transform.position, GetComponent<Rigidbody2D>().velocity, Color.blue);
        // Debug.DrawRay(this.transform.position, transform.up, Color.magenta);
        sensor.AddObservation(currentGate.Value.localPosition);
        sensor.AddObservation(GetComponent<Rigidbody2D>().velocity);
        sensor.AddObservation(currentGate.Value.localPosition - this.transform.localPosition);
        sensor.AddObservation(this.transform.localRotation);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.transform.up);

        Vector3 m_DirToTarget = currentGate.Value.localPosition - this.transform.localPosition;
        Quaternion m_LookRotation = Quaternion.LookRotation(m_DirToTarget);
        m_TargetDirMatrix = Matrix4x4.TRS(Vector3.zero, m_LookRotation, Vector3.one);

        var bodyUpRelativeToLookRotationToTarget = m_TargetDirMatrix.inverse.MultiplyVector(this.transform.up);
        sensor.AddObservation(bodyUpRelativeToLookRotationToTarget);
    }

    public override void OnActionReceived(float[] vectorAction)
    {

        // Save distance from gate before moving
        // Vector2 posCurrentGateV2 = new Vector2(currentGate.Value.position.x, currentGate.Value.position.y);
        // float distance = (posCurrentGateV2 - GetComponent<Rigidbody2D>().velocity).sqrMagnitude;
        // float distance = (currentGate.Value.localPosition - this.transform.localPosition).sqrMagnitude;
        if (vectorAction[0] > 0) controller.Accelerate();
        else if (vectorAction[0] < 0) controller.Brake();
        // Debug.Log(vectorAction[0]);
        // Debug.Log(vectorAction[1]);
        controller.Steer(Mathf.Clamp(vectorAction[1], -1f, 1f));

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
        // if ((posCurrentGateV2 - GetComponent<Rigidbody2D>().velocity).sqrMagnitude < distance)
        // if ((currentGate.Value.localPosition - this.transform.localPosition).sqrMagnitude < distance)
        // {
        //     AddReward(0.001f);
        // }
        // else
        // {
        //     AddReward(-0.0015f);
        // }

        float m_MovingTowardsDot = Vector3.Dot(GetComponent<Rigidbody2D>().velocity, (currentGate.Value.localPosition - this.transform.localPosition).normalized);
        AddReward(0.03f * m_MovingTowardsDot);

        float m_FacingDot = Vector3.Dot((currentGate.Value.localPosition - this.transform.localPosition).normalized, this.transform.up);
        AddReward(0.01f * m_FacingDot);

        Debug.DrawLine(this.transform.position, currentGate.Value.position, Color.yellow);
        // If touched rewardGate -> reward and move to next gate
        if (GetComponent<Collider2D>().IsTouching(currentGate.Value.GetComponent<Collider2D>()))
        {
            // If gate == finish line more rewards
            // if (currentGate.Equals(currentGate.List.First)) AddReward(0.5f);
            AddReward(0.5f);
            // else AddReward(0.1f);
            // After touching proceed to next gate
            currentGate = currentGate.Equals(currentGate.List.Last) ? currentGate.List.First : currentGate.Next;
        }

        if (controller.State.GetType() == typeof(OffTrackState))
        {
            AddReward(-1f);
            EndEpisode();
        }

        /*
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
                    AddReward(-0.5f);
                    EndEpisode();
                }
            }
        }*/
        //^^^^
        // if (controller.GetComponent<Collider2D>().IsTouching(trackCollider)) AddReward(0.001f);
        // else AddReward(-0.001f);

        // Punishment for taking time
        // if (maxStep > 0) AddReward(-1f / maxStep);
        AddReward(-0.001f);
    }

    public override float[] Heuristic()
    {
        float[] actions = new float[2];
        if (Input.GetButton("Break")) actions[0] = -1;
        else if (Input.GetButton("Accelerate")) actions[0] = 1;
        // actions[0] = 1f;

        actions[1] = Input.GetAxis("Horizontal");
        // actions[1] = Conduire();

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

    private float Conduire()
    {
        //Calcul de l'angle du prochain noeud du tracé en fonction de la position du véhicule
        Vector3 relativeVector = transform.InverseTransformPoint(currentGate.Value.position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * 45f;
        //Ajustement du virage
        newSteer = AjusterVirage(newSteer);


        return newSteer;
    }

    private float AjusterVirage(float newSteer)
    {
        if (InRange(newSteer, -.1f, .1f)) newSteer = 0;
        else if (InRange(newSteer, -.5f, .5f)) newSteer = .05f * Mathf.Sign(newSteer);
        else if (InRange(newSteer, -1f, 1f)) newSteer = .125f * Mathf.Sign(newSteer);
        else if (InRange(newSteer, -1.5f, 1.5f)) newSteer = .25f * Mathf.Sign(newSteer);
        else if (InRange(newSteer, -2f, 2f)) newSteer = .5f * Mathf.Sign(newSteer);

        return newSteer;
    }

    private bool InRange(float value, float min, float max)
    {
        return min <= value && value <= max;
    }
}