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

        FooterController.InitializeFooter(Root, pagesManager);
    }

    private void GenerateCategories(PagesManager pagesManager)
    {
        // Get the container where the category buttons will be placed
        var categoryList = Root.Q<VisualElement>("CategorysList");

        // Ensure the container is not null
        if (categoryList == null)
        {
            Debug.LogError("CategorysList container not found in WelcomePage.");
            return;
        }

        // Create categories (in a real scenario, these would likely be loaded dynamically or from data)
        var categories = new List<(string title, string imageName)>
        {
            ("Bureau", "bureau-img.png"),
            ("Kitchen", "kitchen-img.png"),
            ("Room", "room-img.png")
        };

        foreach (var category in categories)
        {
            // Create a new VisualElement for the category card
            var categoryCard = new VisualElement();

            // Create and configure the image VisualElement
            var categoryImg = new VisualElement();
            categoryImg.style.flexGrow = 1;
            categoryImg.style.backgroundImage = new StyleBackground(Resources.Load<Texture2D>("UI/Imgs/" + category.imageName));
            categoryCard.Add(categoryImg);

            // Create and configure the title Label
            var categoryTitle = new Label(category.title)
            {
                style = {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    marginTop = 5,
                    fontSize = 16,
                    color = new StyleColor(Color.white),
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };
            categoryCard.Add(categoryTitle);

            // Register click event for the category card
            categoryCard.RegisterCallback<ClickEvent>(evt =>
            {
                Debug.Log($"Category {category.title} clicked."); // Debug log for category click event
                pagesManager.ShowCategoryPage(category.title);
            });

            // Add the card to the container
            categoryList.Add(categoryCard);
        }
    }
}