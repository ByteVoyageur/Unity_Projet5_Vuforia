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
    var categoryList = Root.Q<ScrollView>("CategorysScrollList");

    // Load the category card template
    if (!pagesManager.pageAssets.TryGetValue("CategoryCartTemplate", out var categoryCartTemplate))
    {
        Debug.LogError("CategoryCartTemplate not found in pageAssets.");
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

        // Create a new VisualElement from the template
        var categoryCardInstance = categoryCartTemplate.CloneTree();

        // Configure the image VisualElement
        var categoryImg = categoryCardInstance.Q<VisualElement>("CategoryCartImage");
        if (categoryImg != null && categoryImage != null)
        {
            categoryImg.style.backgroundImage = new StyleBackground(categoryImage);
        }

        // Configure the title Label
        var categoryTitle = categoryCardInstance.Q<Label>("CategoryTitle");
        if (categoryTitle != null)
        {
            categoryTitle.text = categoryName;
        }

        // Register click event for the category card
        categoryCardInstance.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log($"Category {categoryName} clicked."); // Debug log for category click event
            pagesManager.ShowCategoryPage(categoryName);
        });

        // Add the card to the container
        categoryList.Add(categoryCardInstance);
    }
}
}