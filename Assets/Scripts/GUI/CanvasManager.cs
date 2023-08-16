using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI Jlabel;
    public TextMeshProUGUI Klabel;
    public TextMeshProUGUI Nlabel;

    private void Start() {
        _lastSize = MainSyncSwarm.Instance.Agents.Count;
        Nlabel.text = _lastSize.ToString();
    }

    void Update() {
        Jlabel.text = MainSyncSwarm.Instance.J.ToString();
        Klabel.text = MainSyncSwarm.Instance.K.ToString();
        if (MainSyncSwarm.Instance.Agents.Count != _lastSize) {
            _lastSize = MainSyncSwarm.Instance.Agents.Count;
            Nlabel.text = _lastSize.ToString();
        }
    }

    private int _lastSize;
}
