using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WelcomePage : Page
{
    public WelcomePage(VisualTreeAsset visualTreeAsset) : base(visualTreeAsset)
    {
    }

    public static WelcomePage CreateInstance(VisualTreeAsset visualTreeAsset)
    {
        return new WelcomePage(visualTreeAsset);
    }

    public void Initialize(PagesManager pagesManager)
    {
        GenerateCategories(pagesManager);
        
        // Find HomeFooter element and add click event to navigate back to HomePage
        var homeFooter = Root.Q<VisualElement>("HomeFooter");
        if (homeFooter != null)
        {
            homeFooter.RegisterCallback<ClickEvent>(evt =>
            {
                pagesManager.ShowPage("HomePage");
            });
        }
        else
        {
            Debug.LogError("HomeFooter element not found on WelcomePage.");
        }
    }

    private void GenerateCategories(PagesManager pagesManager)
{
    // Get the container where the category buttons will be placed
    var buttonContainer = Root.Q<VisualElement>("ButtonContainer");

    // Ensure the container is not null
    if (buttonContainer == null)
    {
        Debug.LogError("ButtonContainer not found in WelcomePage.");
        return;
    }

    // Create categories (in a real scenario, these would likely be loaded dynamically or from data)
    var categories = new List<string> { "Bureau", "Kitchen", "Room" };
    
    foreach (var category in categories)
    {
        var button = new Button { text = category };
        button.clickable.clicked += () =>
        {
            Debug.Log($"Button {category} clicked."); // Debug log for button click event
            pagesManager.ShowCategoryPage(category);
        };
        buttonContainer.Add(button);
    }
}
}