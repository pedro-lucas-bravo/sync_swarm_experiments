using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public abstract class SyncAgent : MonoBehaviour
{
    public int ID = -1;
    public Vector3 Position { get => trans_.position; }
    public float Phase { get; protected set; }
    public float Angle { get; protected set; }

    protected virtual void Awake() {
        trans_ = transform;
        material_ = GetComponent<Renderer>().material;
    }

    protected Transform trans_;
    protected Material material_;


}
