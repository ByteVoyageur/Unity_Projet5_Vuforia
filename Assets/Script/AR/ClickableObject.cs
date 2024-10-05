using System.Collections;
using UnityEngine;
using Lean.Touch;

public class ClickableObject : MonoBehaviour
{
    public InfoPanelHandler infoPanelHandler;
    private bool isHolding = false;
    private float holdStartTime;
    private float holdThreshold = 0.5f; 

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUp += HandleFingerUp;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= HandleFingerDown;
        LeanTouch.OnFingerUp -= HandleFingerUp;
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        if (IsFingerOverThisObject(finger))
        {
            holdStartTime = Time.time;
            isHolding = true;
        }
    }

    private void HandleFingerUp(LeanFinger finger)
    {
        if (isHolding)
        {
            isHolding = false;
            float holdDuration = Time.time - holdStartTime;
            if (holdDuration >= holdThreshold)
            {
                ShowInfoPanelIfFingerOverThisObject(finger);
            }
        }
    }

    private bool IsFingerOverThisObject(LeanFinger finger)
    {
        Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            return hitInfo.collider.gameObject == gameObject;
        }
        return false;
    }

    private void ShowInfoPanelIfFingerOverThisObject(LeanFinger finger)
    {
        Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.gameObject == gameObject && infoPanelHandler != null)
            {
                infoPanelHandler.ShowInfoPanel();
            }
        }
    }
}