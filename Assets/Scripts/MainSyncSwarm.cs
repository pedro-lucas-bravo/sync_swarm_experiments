﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainSyncSwarm : MonoBehaviour
{
    [Header("a Static sync state for (J, K) = (0.1, 1). " +
        "\nb Static async state (J, K) = (0.1, −1). " +
        "\nc Static phase wave state (J, K) = (1, 0)). " +
        "\na Splintered phase wave (J, K) = (1, −0.1), " +
        "\nb Active phase wave (J, K) = (1, −0.75)")]
    public Swarmalator AgentPrefab;
    public int Size = 10;
    public float J = 0.1f;
    public float K = 1.0f;
    public float DeltaFactor = 1f;
    public Transform reference;

    public List<Swarmalator> Agents = new List<Swarmalator>();

    public static MainSyncSwarm Instance { get; private set; }

    private int _lastSize;

    // Start is called before the first frame update
    void Awake(){
        Instance = this;        
    }

    private void Start() {
        Instatiate(Size);
        _lastSize = Size;
    }

    private void Update() {
        //Update parameters per agent
        for (int i = 0; i < Agents.Count; i++) {
            Agents[i].J = J;
            Agents[i].K = K;
            Agents[i].DeltaFactor = DeltaFactor;
        }

        //Transform reference update
        (var center, var normalPlane) = BestFitNormal(Agents.Select(a => a.Position));
        reference.position = center;
        reference.forward = normalPlane;

        //Real-time instantiation
        if (_lastSize != Size) {
            Instatiate(Size);
            _lastSize = Size;
        }

        //Add 1
        if (Input.GetKeyDown(KeyCode.A)){
            Size++;
            _lastSize = Size;
            Add(1);
        }

        //Remove 1
        if (Input.GetKeyDown(KeyCode.R) && Size > 3) { //At least size to for right calculation of the reference
            Size--;
            _lastSize = Size;
            Remove(1);
        }
    }

    //(center, normalPlane)
    (Vector3, Vector3) BestFitNormal(IEnumerable<Vector3> points) {
        Vector3 pointAverage = Vector3.zero;

        foreach (Vector3 point in points) {
            pointAverage += point;
        }

        pointAverage /= points.Count(); // Get the average point

        // Build the covariance matrix
        float xx = 0.0f, xy = 0.0f, xz = 0.0f, yy = 0.0f, yz = 0.0f, zz = 0.0f;

        foreach (Vector3 point in points) {
            Vector3 r = point - pointAverage;
            xx += r.x * r.x;
            xy += r.x * r.y;
            xz += r.x * r.z;
            yy += r.y * r.y;
            yz += r.y * r.z;
            zz += r.z * r.z;
        }

        float det_x = yy * zz - yz * yz;
        float det_y = xx * zz - xz * xz;
        float det_z = xx * yy - xy * xy;

        // Pick the direction with the largest determinant
        if (det_x > det_y && det_x > det_z)
            return (pointAverage, new Vector3(det_x, xz * yz - xy * zz, xy * yz - xz * yy).normalized);
        if (det_y > det_z)
            return (pointAverage, new Vector3(xz * yz - xy * zz, det_y, xy * xz - yz * xx).normalized);
        else
            return (pointAverage, new Vector3(xy * yz - xz * yy, xy * xz - yz * xx, det_z).normalized);
    }

    public void Instatiate(int size) {
        Remove(Agents.Count);
        Add(size);
        ExternalCommunicationManager.Instance.InstantiateAgent(size);
    }

    public void Add(int size) {
        var init = Agents.Count;
        for (int i = init; i < init + size; i++) {
            var agent = Instantiate<Swarmalator>(AgentPrefab);
            agent.ID = i + 1;
            agent.J = J;
            agent.K = K;
            agent.name = "Agent_" + i;
            agent.transform.SetParent(transform);
            Agents.Add(agent);
        }
        AdjustIndexesAndNeighbours();
        ExternalCommunicationManager.Instance.AddAgent(size);
    }

    public void Remove(int size) {
        for (int i = 0; i < size; i++) {
            var agentToRemove = Agents[0];
            DestroyImmediate(agentToRemove.gameObject);
            Agents.RemoveAt(0);
        }
        AdjustIndexesAndNeighbours();
        ExternalCommunicationManager.Instance.RemoveAgent(size);
    }

    public void AdjustIndexesAndNeighbours() {
        for (int i = 0; i < Agents.Count; i++) {
            Agents[i].ID = i + 1;
            Agents[i].name = "Agent_" + i;
            Agents[i].PopulateAgents();
        }
    }

    
}
