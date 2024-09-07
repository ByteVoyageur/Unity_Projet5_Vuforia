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
    var itemsContainer = Root.Q<ScrollView>("CategoryItemsScrollContainer");

    if (itemsContainer == null)
    {
        Debug.LogError("CategoryItemsScrollContainer not found in CategoryPage.");
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

        var itemImg = itemElement.Q<VisualElement>("Image");
        if (itemImg != null && item.icon != null)
        {
            itemImg.style.backgroundImage = new StyleBackground(item.icon);
            Debug.Log($"Set item image for {item.itemName}");
        }
        else
        {
            Debug.LogWarning($"Item image or categoryImage is null for item {item.itemName}");
        }

        var itemTitle = itemElement.Q<Label>("TitleCart");
        if (itemTitle != null)
        {
            itemTitle.text = item.itemName;
            Debug.Log($"Set item title for {item.itemName}");
        }
        else
        {
            Debug.LogWarning($"Item title Label not found for item {item.itemName}");
        }

        var itemDescription = itemElement.Q<Label>("DescritptionCart");
        if (itemDescription != null)
        {
            itemDescription.text = item.description;
            Debug.Log($"Set item description for {item.itemName}");
        }
        else
        {
            Debug.LogWarning($"Item description Label not found for item {item.itemName}");
        }

        var itemPrice = itemElement.Q<Label>("Price");
        if (itemPrice != null)
        {
            itemPrice.text = $"${item.price}";
            Debug.Log($"Set item price for {item.itemName}");
        }
        else
        {
            Debug.LogWarning($"Item price Label not found for item {item.itemName}");
        }

        // Register click event for each item card
        itemElement.RegisterCallback<ClickEvent>(evt =>
        {
            pagesManager.ShowItemDetailPage(item);
        });

        itemsContainer.Add(itemElement);
    }
}
}