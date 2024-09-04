using UnityEngine;
using UnityEngine.UIElements;

public class CategoryPage : Page
{
    public CategoryPage(VisualTreeAsset visualTreeAsset) : base(visualTreeAsset)
    {
    }

    public static CategoryPage CreateInstance(VisualTreeAsset visualTreeAsset)
    {
        return new CategoryPage(visualTreeAsset);
    }

    public void Initialize(PagesManager pagesManager)
    {
        // Find CategoryItem element and add click event to navigate to ItemDetailPage
        var categoryItem = Root.Q<VisualElement>("CategoryItem");

        if (categoryItem != null)
        {
            categoryItem.RegisterCallback<ClickEvent>(evt =>
            {
                pagesManager.ShowPage("ItemDetailPage");
            });
        }
        else
        {
            Debug.LogError("CategoryItem element not found on CategoryPage.");
        }
    }
}