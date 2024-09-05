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

        // Load all CategorySO assets from the Resources/Data/Category directory
        var categoryAssets = Resources.LoadAll<CategorySO>("Data/Category");

        if (categoryAssets == null || categoryAssets.Length == 0)
        {
            Debug.LogError("No CategorySO assets found in Data/Category directory.");
            return;
        }

        foreach (var categorySO in categoryAssets)
        {
            var categoryName = categorySO.name; // Assuming the name of scriptable object is the name of the category
            var categoryImage = categorySO.categoryImage; // Get the image directly from the CategorySO

            // Ensure categoryName and categoryImage are valid
            if (string.IsNullOrEmpty(categoryName))
            {
                Debug.LogError("CategorySO name is missing.");
                continue;
            }

            if (categoryImage == null)
            {
                Debug.LogWarning($"Category {categoryName} does not have a valid image.");
            }

            // Create a new VisualElement for the category card
            var categoryCard = new VisualElement
            {
                style = { flexDirection = FlexDirection.Column }
            };

            // Create and configure the image VisualElement
            var categoryImg = new VisualElement
            {
                style = { flexGrow = 1 }
            };
            if (categoryImage != null)
            {
                categoryImg.style.backgroundImage = new StyleBackground(categoryImage);
            }
            categoryCard.Add(categoryImg);

            // Create and configure the title Label
            var categoryTitle = new Label(categoryName)
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
                Debug.Log($"Category {categoryName} clicked."); // Debug log for category click event
                pagesManager.ShowCategoryPage(categoryName);
            });

            // Add the card to the container
            categoryList.Add(categoryCard);
        }
    }
}