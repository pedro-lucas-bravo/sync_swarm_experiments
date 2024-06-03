using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI Jlabel;
    public TextMeshProUGUI Klabel;
    public TextMeshProUGUI vlabel;
    public TextMeshProUGUI Nlabel;
    public TextMeshProUGUI stateLabel;
    public TextMeshProUGUI rhythmLabel;
    public TextMeshProUGUI scaleLabel;
    public Color scaleOnColor;
    public Color scaleOffColor;

    private int _lastSize;
    private float _oldJ = float.PositiveInfinity;
    private float _oldK = float.PositiveInfinity;
    private float _oldV = float.PositiveInfinity;

    private void Start() {
        _lastSize = MainSyncSwarm.Instance.Agents.Count;
        Nlabel.text = _lastSize.ToString();
        MainSyncSwarm.Instance.OnChangeRhythmMode += OnChangeRhythmMode;
        MainSyncSwarm.Instance.OnChangeScaleID += OnChangeScaleID;
    }

    private void OnDestroy() {
        if (MainSyncSwarm.Instance != null) {
            MainSyncSwarm.Instance.OnChangeRhythmMode -= OnChangeRhythmMode;
            MainSyncSwarm.Instance.OnChangeScaleID -= OnChangeScaleID;
        }
    }

    private void OnChangeScaleID(int scaleID) {
        string scaleName;
        switch (scaleID) {
            case 1:
                scaleName = "Major";
                break;
            case 2:
                scaleName = "Pentatonic Major";
                break;
            case 3:
                scaleName = "Minor";
                break;
            case 4:
                scaleName = "Pentatonic Minor";
                break;
            case 5:
                scaleName = "Blues";
                break;
            default:
                scaleName = "Custom";
                break;
        }
        scaleLabel.text = scaleName;
    }

    private void OnChangeRhythmMode(int mode) {
        rhythmLabel.color = mode == 1 ? scaleOnColor : scaleOffColor;
        rhythmLabel.text = mode == 1 ?"ON": "OFF";
    }

    void Update() {
        if (MainSyncSwarm.Instance.J != _oldJ || MainSyncSwarm.Instance.K != _oldK) {
            stateLabel.text = JK_to_State(MainSyncSwarm.Instance.J, MainSyncSwarm.Instance.K);
        }
        if (MainSyncSwarm.Instance.J != _oldJ) {
            _oldJ = MainSyncSwarm.Instance.J;
            Jlabel.text = _oldJ.ToString();
        }
        if (MainSyncSwarm.Instance.K != _oldK) {
            _oldK = MainSyncSwarm.Instance.K;
            Klabel.text = _oldK.ToString();
        }
        if (MainSyncSwarm.Instance.DeltaFactor != _oldV) {
            _oldV = MainSyncSwarm.Instance.DeltaFactor;
            vlabel.text = _oldV.ToString();
        }
        if (MainSyncSwarm.Instance.Agents.Count != _lastSize) {
            _lastSize = MainSyncSwarm.Instance.Agents.Count;
            Nlabel.text = _lastSize.ToString();
        }        
    }

    private string JK_to_State(float J, float K) {
        if(Mathf.Approximately(J, 0.1f) && Mathf.Approximately(K, 1f)) {
            return "1. static sync";
        }
        if(Mathf.Approximately(J, 0.1f) && Mathf.Approximately(K, -1f)) {
            return "2. static A-sync";
        }
        if(Mathf.Approximately(J, 1f) && Mathf.Approximately(K, 0f)) {
            return "3. static phase wave";
        }
        if(Mathf.Approximately(J, 1f) && Mathf.Approximately(K, -0.1f)) {
            return "4. splintered phase wave";
        }
        if(Mathf.Approximately(J, 1f) && Mathf.Approximately(K, -0.75f)) {
            return "5. active phase wave";
        }
        return "custom (J, K)...";
    }
    
}
