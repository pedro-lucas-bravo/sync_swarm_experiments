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
    public string instantiateAddress = "/instantiate"; // N to instantiate
    public string addSendAddress = "/add"; // N to add
    public string removeSendAddress = "/remove"; // N to remove

    [Header("Addresses to receive")]
    public string JparamAddress = "/J";
    public string KparamAddress = "/K";
    public string deltaFactorAddress = "/deltafactor";
    public string addRecvAddress = "/add"; // N to add
    public string removeRecvAddress = "/remove"; // N to remove
    public string resizeRecvAddress = "/resize"; // N to resize
    public string addManualRecvAddress = "/addmanual"; // N to add
    public string removeManualRecvAddress = "/removemanual"; // N to remove

    //[Header("Configurations")]
    //public int sleepMillisecondsOnReceiving = 0;

    private void Awake() {
        if (Instance == null)
            Instance = this;
    }

    private void Start() {
        //Sender Messages
        allAgentInfoOutMessage_ = OSC.DefineMessageToClient(allAgentInfoAddress,2);
        instantiateOutMessage_ = OSC.DefineMessageToClient(instantiateAddress, 1);
        addOutMessage_ = OSC.DefineMessageToClient(addSendAddress, 1);
        removeOutMessage_ = OSC.DefineMessageToClient(removeSendAddress, 1);

        //Receivers        
        OSC.OnReceiveMessage += OnReceive;

        //SetSleepMillisecons(sleepMillisecondsOnReceiving);
    }

    private void OnDestroy() {
        if (OSC != null) {
            OSC.OnReceiveMessage -= OnReceive;
        }
        Instance = null;
    }

    private void LateUpdate() {
        var deltaTime = Time.deltaTime;
        SendAgentsInfo(deltaTime);
        AddRecvAgent();
        RemoveRecvAgent();
        AddManualRecvAgent();
        RemoveManualRecvAgent();

    }

    List<object> allAgentInfoOutMessage_;
    List<object> instantiateOutMessage_;
    List<object> addOutMessage_;
    List<object> removeOutMessage_;

    float _testSensorValue;

    #region Senders

    StringBuilder stringAgentsInfo = new StringBuilder();
    float timerAgentsInfoSend_ = 0;
    float periodInfoSend_ = 0;
    public void SendAgentsInfo(float deltaTime) {
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

    public void InstantiateAgent(int size) {
        if (OSCHandler.Instance.Clients.Any()) {
            instantiateOutMessage_[0] = size;
            OSC.SendMessageToClient(instantiateAddress);
        }
    }

    public void AddAgent(int size) {
        if (OSCHandler.Instance.Clients.Any()) {
            addOutMessage_[0] = size;
            OSC.SendMessageToClient(addSendAddress);
        }
    }

    public void RemoveAgent(int size) {
        if (OSCHandler.Instance.Clients.Any()) {
            removeOutMessage_[0] = size;
            OSC.SendMessageToClient(removeSendAddress);
        }
    }

    #endregion

    #region Receivers
    private void OnReceive(string address, List<object> values, OSCPacket packet) {
        if (address == JparamAddress) {
            MainSyncSwarm.Instance.J = (float)values[0];
        }
        if (address == KparamAddress) {
            MainSyncSwarm.Instance.K = (float)values[0];
        }
        if (address == deltaFactorAddress) {
            MainSyncSwarm.Instance.DeltaFactor = (float)values[0];
        }
        if (address == addRecvAddress && !_AddAgentFlag) {
            _AddAgentFlag = true;
        }
        if (address == removeRecvAddress && !_RemoveAgentFlag) {
            _RemoveAgentFlag = true;
        }
        if (address == addManualRecvAddress && !_AddManualAgentFlag) {
            _AddManualAgentFlag = true;
        }
        if (address == removeManualRecvAddress && !_RemoveManualAgentFlag) {
            _RemoveManualAgentFlag = true;
        }
        if (address == resizeRecvAddress) {
            MainSyncSwarm.Instance.Size = (int)values[0];
        }
    }

    private bool _AddAgentFlag;
    private void AddRecvAgent() { //TODO: Just one for now
        if (_AddAgentFlag) {
            MainSyncSwarm.Instance.AddOne();
            _AddAgentFlag = false;
        }
    }

    private bool _RemoveAgentFlag;
    private void RemoveRecvAgent() { //TODO: Just one for now
        if (_RemoveAgentFlag) {
            MainSyncSwarm.Instance.RemoveOne();
            _RemoveAgentFlag = false;
        }
    }

    private bool _AddManualAgentFlag;
    private void AddManualRecvAgent() { //TODO: Just one for now
        if (_AddManualAgentFlag) {
            MainSyncSwarm.Instance.Add(1, true);
            _AddManualAgentFlag = false;
        }
    }

    private bool _RemoveManualAgentFlag;
    private void RemoveManualRecvAgent() { //TODO: Just one for now
        if (_RemoveManualAgentFlag) {
            MainSyncSwarm.Instance.Remove(1, true);
            _RemoveManualAgentFlag = false;
        }
    }

    #endregion
}
