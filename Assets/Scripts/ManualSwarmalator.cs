using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class ManualSwarmalator : SyncAgent
{

    protected override void Awake() {
        base.Awake();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            float currentTime = Time.time;
            float period = currentTime - lastClickTime;
            frequency = 1f / period;
            phase = currentTime * frequency;// phae in herz
            Phase = 2 * Mathf.PI * phase;//phase in radians
            lastClickTime = currentTime;
        }

        var pulse = Mathf.Sin(2 * Mathf.PI * (frequency * Time.time + phase));
        var color = Color.red;
        material_.color = Color.Lerp(color, Color.black, Mathf.InverseLerp(-1f, 1f, pulse));
    }

    float lastClickTime;
    float frequency = 1;
    float phase = 0;
}
