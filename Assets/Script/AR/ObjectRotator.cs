using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class ObjectRotator : MonoBehaviour
{
    private bool isDragging = false;
    private bool isRotating = false;
    private Vector3 lastFramePosition;

    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += HandleFingerUpdate;
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUp += HandleFingerUp;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
        LeanTouch.OnFingerDown -= HandleFingerDown;
        LeanTouch.OnFingerUp -= HandleFingerUp;
    }

    private void HandleFingerUpdate(LeanFinger finger)
    {
        if (finger.IsOverGui)
        {
            return;
        }

        if (isDragging)
        {
            Vector3 currentFramePosition = finger.ScreenPosition;
            Vector3 direction = currentFramePosition - lastFramePosition;
            lastFramePosition = currentFramePosition;

            float angle = direction.x * 0.1f; // Adjust rotation sensitivity here
            transform.Rotate(Vector3.up, -angle, Space.World);
        }
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        if (isRotating)
        {
            isRotating = false;
            isDragging = false;
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
        {
            isDragging = true;
            isRotating = true;
            lastFramePosition = finger.ScreenPosition;
        }
    }

    private void HandleFingerUp(LeanFinger finger)
    {
        isDragging = false;
    }
}