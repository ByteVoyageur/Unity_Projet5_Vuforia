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
            itemImg.style.backgroundImage = new StyleBackground(item.icon);

        var itemTitle = itemElement.Q<Label>("TitleCart");
            itemTitle.text = item.itemName;

        var itemDescription = itemElement.Q<Label>("DescritptionCart");
            itemDescription.text = item.description;

        var itemPrice = itemElement.Q<Label>("Price");
            itemPrice.text = $"${item.price}";

        // Register click event for each item card
        itemElement.RegisterCallback<ClickEvent>(evt =>
        {
            pagesManager.ShowItemDetailPage(item);
        });

        itemsContainer.Add(itemElement);
    }
}
}