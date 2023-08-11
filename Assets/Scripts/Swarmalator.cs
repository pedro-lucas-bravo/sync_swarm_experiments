using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Swarmalator : SyncAgent
{
    #region Public Interface

    //Initial state is static sync (a Static sync state for (J, K) = (0.1, 1). b Static async state (J, K) = (0.1, −1). c Static phase wave state (J, K) = (1, 0))
    //a Splintered phase wave (J, K) = (1, −0.1), b Active phase wave (J, K) = (1, −0.75)
    [Header("Swarmalator Balance")]
    public float J = 0.1f;
    public float K = 1.0f;
    public float DeltaFactor = 1.0f;

    [Header("Pulse Balance")]
    public float amplitude = 1.0f;
    public float frequency = 1.0f;

    #endregion


    protected override void Awake() {
        base.Awake();
        Phase = Random.Range(0, 2 * Mathf.PI);
        trans_.position = Random.insideUnitSphere * Mathf.PI;        
    }

    // Start is called before the first frame update
    void Start(){
        PopulateAgents();
    }

    public void PopulateAgents() {
        Agents = FindObjectsOfType<SyncAgent>().Where(s => s != this).ToArray();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        (var Xi, var Oi) = DeltaCalculations();
        trans_.position += Xi * DeltaTime;
        Phase += Oi * DeltaTime;
        Phase = Phase % (2 * Mathf.PI);
        Phase = Phase < 0 ? Phase + 2 * Mathf.PI : Phase;
        var color = Color.HSVToRGB(Mathf.InverseLerp(0, 2 * Mathf.PI, Phase), 1, 1);
        var pulse = amplitude * Mathf.Sin(2 * Mathf.PI * frequency * Time.fixedTime + Phase);
        material_.color = Color.Lerp(color, Color.black, Mathf.InverseLerp(-amplitude, amplitude, pulse));
    }

    void Update() {
        var reference = MainSyncSwarm.Instance.reference;
        Angle = Vector3.SignedAngle(trans_.position - reference.position, reference.right, reference.forward);
        Angle = Angle < 0 ? Angle + 360.0f : Angle;
    }

    (Vector3, float) DeltaCalculations() {
        Vector3 Xi = Vector3.zero;
        float Oi = 0;
        for (int j = 0; j < Agents.Length; j++) {
            var Xji = Agents[j].Position - Position;
            var Oji = Agents[j].Phase - Phase + Random.Range(0, 0.0001f);
            Xi += Xji / Xji.magnitude * (1 + J * Mathf.Cos(Oji)) - Xji / Mathf.Pow(Xji.magnitude, 3);
            Oi += Mathf.Sin(Oji) / Xji.magnitude;
        }
        var N = Agents.Length + 1;// +1 because agents does not contains "this" agent
        Xi /= N;
        Oi *= (K / N);
        return (Xi, Oi);
    }

    float DeltaTime => Time.fixedDeltaTime* DeltaFactor;

    SyncAgent[] Agents;
}
