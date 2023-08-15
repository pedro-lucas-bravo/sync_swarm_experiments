using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGrabber : MonoBehaviour
{
    public string TargetTag;
    public bool IsDragging => isDragging;

    private bool isDragging = false;
    private Vector3 originalPosition;
    private Vector3 offset;
    private GameObject draggedCube;
    private Plane dragPlane;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (!isDragging) {
                // Check if we are clicking on a cube
                Vector3 normal;
                draggedCube = GetCubeUnderMouse(out normal);
                if (draggedCube != null) {
                    originalPosition = draggedCube.transform.position;
                    dragPlane = new Plane(normal, originalPosition);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float distance;
                    if (dragPlane.Raycast(ray, out distance)) {
                        offset = originalPosition - ray.GetPoint(distance);
                    }
                    isDragging = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (isDragging) {
                isDragging = false;
                draggedCube = null;
            }
        }

        if (isDragging && draggedCube != null) {
            // Update the cube's position based on the mouse movement on the face plane
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (dragPlane.Raycast(ray, out distance)) {
                Vector3 targetPosition = ray.GetPoint(distance) + offset;
                draggedCube.transform.position = targetPosition;
            }
        }
    }

    private GameObject GetCubeUnderMouse(out Vector3 normal) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits) {
            if (hit.collider.CompareTag(TargetTag)) {
                normal = hit.normal;
                return hit.collider.gameObject;
            }
        }
        normal = Vector3.zero;
        return null;
    }
}
