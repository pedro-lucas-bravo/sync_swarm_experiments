using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Awake(){
        for (int i = 0; i < Size; i++) {
            var agent = Instantiate<Swarmalator>(AgentPrefab);
            agent.J = J;
            agent.K = K;
            agent.name = "Agent_" + i;
            agent.transform.SetParent(transform);
            Agents.Add(agent);
        }
    }

    private void Update() {
        for (int i = 0; i < Agents.Count; i++) {
            Agents[i].J = J;
            Agents[i].K = K;
            Agents[i].DeltaFactor = DeltaFactor;
        }
    }

    List<Swarmalator> Agents = new List<Swarmalator>();
}
