using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityOSC;

public class ExternalCommunicationManager : MonoBehaviour
{
    public static ExternalCommunicationManager Instance { get; private set; }

    public OscManager OSC;

    [Header("Addresses to send")]
    public string allAgentInfoAddress = "/syncagents"; //size, string[id0, phase0, x0, y0, z0, id0, phase1, x1, y1, z1....]

    //[Header("Configurations")]
    //public int sleepMillisecondsOnReceiving = 0;

    private void Awake() {
        if (Instance == null)
            Instance = this;
    }

    private void Start() {
        //Sender Messages
        allAgentInfoOutMessage_ = OSC.DefineMessageToClient(allAgentInfoAddress,2);

        //Receivers        
        //OSC.OnReceiveMessage += OnReceive;

        //SetSleepMillisecons(sleepMillisecondsOnReceiving);
    }

    private void OnDestroy() {
        //if (OSC != null) {
        //    OSC.OnReceiveMessage -= OnReceive;
        //}
        Instance = null;
    }

    private void LateUpdate() {
        var deltaTime = Time.deltaTime;
        SendStimergicObjectsInfo(deltaTime);
    }

    List<object> allAgentInfoOutMessage_;

    float _testSensorValue;

    #region Senders

    StringBuilder stringAgentsInfo = new StringBuilder();
    float timerAgentsInfoSend_ = 0;
    float periodInfoSend_ = 0;
    public void SendStimergicObjectsInfo(float deltaTime) {
        if (OSCHandler.Instance.Clients.Any()) {
            timerAgentsInfoSend_ += deltaTime;
            if (timerAgentsInfoSend_ >= periodInfoSend_) {
                var stObjects = MainSyncSwarm.Instance.Agents;
                if (stObjects.Count > 0) {
                    allAgentInfoOutMessage_[0] = stObjects.Count;
                    stringAgentsInfo.Clear();
                    for (int i = 0; i < stObjects.Count; i++) {
                        stringAgentsInfo.Append(stObjects[i].ID);
                        stringAgentsInfo.Append(' ');
                        stringAgentsInfo.Append(Mathf.RoundToInt(stObjects[i].Phase * 1000));
                        stringAgentsInfo.Append(' ');
                        stringAgentsInfo.Append(Mathf.RoundToInt(stObjects[i].Angle * 1000));
                        stringAgentsInfo.Append(' ');
                        stringAgentsInfo.Append(Mathf.RoundToInt(stObjects[i].Position.x * 1000));
                        stringAgentsInfo.Append(' ');
                        stringAgentsInfo.Append(Mathf.RoundToInt(stObjects[i].Position.z * 1000));
                        stringAgentsInfo.Append(' ');
                        stringAgentsInfo.Append(Mathf.RoundToInt(stObjects[i].Position.y * 1000));
                        stringAgentsInfo.Append(' ');
                    }
                    if (stringAgentsInfo.Length > 0)
                        stringAgentsInfo.Remove(stringAgentsInfo.Length - 1, 1);
                    allAgentInfoOutMessage_[1] = stringAgentsInfo.ToString(); // TODO: Optimize so that a new string is not created every time
                    OSC.SendMessageToClient(allAgentInfoAddress);
                }
                timerAgentsInfoSend_ = 0;
            }
        }
    }



    #endregion
    
}
