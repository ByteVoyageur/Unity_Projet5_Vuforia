using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanelHandler : MonoBehaviour
{
    public GameObject InfoPanel;
    public TextMeshProUGUI infoText;
    public FurnitureSO furnitureData;

    void Start()
    {
        // Ensure the panel is initially hidden
        HideInfoPanel();
    }

    public void ShowInfoPanel()
    {
        infoText.text = $"Nom: {furnitureData.itemName}\nDescription: {furnitureData.description}\nPrix: {furnitureData.price}";
        InfoPanel.SetActive(true);
    }

    public void HideInfoPanel()
    {
        InfoPanel.SetActive(false);
    }
}
