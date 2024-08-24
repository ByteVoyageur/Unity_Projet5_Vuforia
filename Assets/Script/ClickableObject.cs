using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public InfoPanelHandler infoPanelHandler;

    void OnMouseDown()
    {
        if (infoPanelHandler != null)
        {
            infoPanelHandler.ShowInfoPanel();
        }
    }
}
