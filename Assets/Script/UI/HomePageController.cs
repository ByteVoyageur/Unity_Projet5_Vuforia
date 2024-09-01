using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HomePageController : MonoBehaviour
{
    private Button laterButton;
    private UIDocument uiDocument;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        laterButton = root.Q<Button>("LaterButton");

        if (laterButton != null)
        {
            laterButton.clicked += OnLaterButtonClicked;
        }
        else
        {
            Debug.LogError("LaterButton not found in the UXML.");
        }
    }

    void OnLaterButtonClicked()
    {
        var visualTree = Resources.Load<VisualTreeAsset>("StoreUIDocuments/WelcomePage"); 
        
        if (visualTree != null)
        {
            var newRoot = visualTree.CloneTree();
            uiDocument.rootVisualElement.Clear(); 
            uiDocument.rootVisualElement.Add(newRoot);
        }
        else
        {
            Debug.LogError("WelcomePage.uxml could not be found in Resources folder.");
        }
    }
}