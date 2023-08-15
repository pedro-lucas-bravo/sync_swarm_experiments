using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI Jlabel;
    public TextMeshProUGUI Klabel;


    void Update() {
        Jlabel.text = MainSyncSwarm.Instance.J.ToString();
        Klabel.text = MainSyncSwarm.Instance.K.ToString();
    }
}
