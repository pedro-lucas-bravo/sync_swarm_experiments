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
            //float period = currentTime - lastClickTime;
            frequency = 1f;// period;
            Phase = -2 * Mathf.PI * currentTime;//phase in radians, assuming starting in 0 and get phi from y = sin(2pi*t + phi) so that phi = -2pi * t
            Phase = Phase % (2 * Mathf.PI);
            Phase = Phase < 0 ? Phase + 2 * Mathf.PI : Phase;
            lastClickTime = currentTime;
        }

        var pulse = Mathf.Sin(2 * Mathf.PI *frequency * Time.time + Phase);
        var color = Color.HSVToRGB(Mathf.InverseLerp(0, 2 * Mathf.PI, Phase), 1, 1);
        material_.color = Color.Lerp(color, Color.black, Mathf.InverseLerp(-1f, 1f, pulse));
    }

    float lastClickTime;
    float frequency = 1;
}
