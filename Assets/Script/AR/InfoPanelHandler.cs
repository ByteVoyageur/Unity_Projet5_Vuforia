using UnityEngine;
using TMPro;

public class InfoPanelHandler : MonoBehaviour
{
    public GameObject InfoPanel; 
    public TextMeshProUGUI infoText;          
    public WishListManager.Item itemData; 

    void Start()
    {
        // Ensure the panel is initially hidden
        HideInfoPanel();
    }

    public void ShowInfoPanel()
    {
    if (itemData != null)
    {
        Debug.Log($"[InfoPanelHandler] Showing info for item: Name={itemData.name}, Description={itemData.description}, Price={itemData.price}");

        if (infoText != null)
        {
            infoText.text = $"Nom: {itemData.name}\nDescription: {itemData.description}\nPrix: {itemData.price}";
            InfoPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("[InfoPanelHandler] infoText is null.");
        }
    }
    else
    {
        Debug.LogWarning("[InfoPanelHandler] itemData is null.");
    }
    }


    public void HideInfoPanel()
    {
        InfoPanel.SetActive(false);
    }
}
