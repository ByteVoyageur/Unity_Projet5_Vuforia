using UnityEngine;
using UnityEngine.UI;

public class ClosePanelButton : MonoBehaviour
{
    public InfoPanelHandler infoPanelHandler;

    void Start()
    {
        Button closeButton = GetComponent<Button>();
        closeButton.onClick.AddListener(ClosePanel);
    }

    void ClosePanel()
    {
        if (infoPanelHandler != null)
        {
            infoPanelHandler.HideInfoPanel();
        }
    }
}