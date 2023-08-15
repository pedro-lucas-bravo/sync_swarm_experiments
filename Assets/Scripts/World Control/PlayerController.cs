using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FirstPersonCamera cameraController;
    public MouseGrabber grabber;

    private void Update() {
        if(grabber != null)
            cameraController.LockToDontMove = grabber.IsDragging;
    }
}
