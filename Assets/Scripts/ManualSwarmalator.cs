using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class ManualSwarmalator : SyncAgent
{

    protected override void Awake() {
        base.Awake();
        _grabber = FindObjectOfType<MouseGrabber>();
        if (_grabber != null)
            _grabber.OnRightClickSelection += OnRightClickSelection;
    }

    private void OnDestroy() {
        if (_grabber != null)
            _grabber.OnRightClickSelection -= OnRightClickSelection;
    }

    private void OnRightClickSelection(GameObject obj) {
        if (obj == gameObject) {
            float currentTime = Time.time;
            //float period = currentTime - lastClickTime;
            _frequency = 1f;// period;
            Phase = -2 * Mathf.PI * currentTime;//phase in radians, assuming starting in 0 and get phi from y = sin(2pi*t + phi) so that phi = -2pi * t
            Phase = Phase % (2 * Mathf.PI);
            Phase = Phase < 0 ? Phase + 2 * Mathf.PI : Phase;
            _lastClickTime = currentTime;
        }
    }

    private void FixedUpdate() {
        var pulse = Mathf.Sin(2 * Mathf.PI * _frequency * Time.fixedTime + Phase);
        pulse = pulse < 0 ? 1.0f : 0.0f;
        var color = Color.HSVToRGB(Mathf.InverseLerp(0, 2 * Mathf.PI, Phase), 1, 1);
        material_.color = Color.Lerp(color, Color.black, Mathf.InverseLerp(-1f, 1f, pulse));
    }

    void Update() {
        var reference = MainSyncSwarm.Instance.reference;
        Angle = Vector3.SignedAngle(trans_.position - reference.position, reference.right, reference.forward);
        Angle = Angle < 0 ? Angle + 360.0f : Angle;
    }

    float _lastClickTime;
    float _frequency = 1;
    MouseGrabber _grabber;
}
