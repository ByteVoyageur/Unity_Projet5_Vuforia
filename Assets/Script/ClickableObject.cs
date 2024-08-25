using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class ClickableObject : MonoBehaviour
{
    public InfoPanelHandler infoPanelHandler;

    private void OnEnable()
    {
        LeanTouch.OnFingerTap += HandleFingerTap;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerTap -= HandleFingerTap;
    }

    private void HandleFingerTap(LeanFinger finger)
    {
        Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.gameObject == gameObject)
            {
                if (infoPanelHandler != null)
                {
                    infoPanelHandler.ShowInfoPanel();
                }
            }
        }
    }
}