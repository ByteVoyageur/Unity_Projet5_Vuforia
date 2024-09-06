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

    public void Initialize(PagesManager pagesManager, CategorySO categoryData)
    {
        GenerateItems(pagesManager, categoryData);
        FooterController.InitializeFooter(Root, pagesManager);
    }

    private void GenerateItems(PagesManager pagesManager, CategorySO categoryData)
    {
        var itemsContainer = Root.Q<VisualElement>("CategoryItemsContainer");

        if (itemsContainer == null)
        {
            Debug.LogError("CategoryItemsContainer not found in CategoryPage.");
            return;
        }

        if (!pagesManager.pageAssets.TryGetValue("ItemCartTemplate", out var itemTemplate))
        {
            Debug.LogError("ItemTemplate not found in pageAssets.");
            return;
        }

        itemsContainer.Clear();

        foreach (var item in categoryData.category)
        {
            var itemElement = itemTemplate.CloneTree();

            var itemImg = itemElement.Q<VisualElement>("CategoryItemImg");
            if (itemImg != null && item.categoryImage != null)
            {
                itemImg.style.backgroundImage = new StyleBackground(item.categoryImage);
            }

            var itemTitle = itemElement.Q<Label>("CategoryItemDescription");
            if (itemTitle != null)
            {
                itemTitle.text = item.itemName;
            }

            var itemPrice = itemElement.Q<Label>("CategoryItemPrice");
            if (itemPrice != null)
            {
                itemPrice.text = $"${item.price}";
            }

            // Register click event for each item card
            itemElement.RegisterCallback<ClickEvent>(evt =>
            {
                pagesManager.ShowPage("ItemDetailPage");
            });

            itemsContainer.Add(itemElement);
        }
    }
}