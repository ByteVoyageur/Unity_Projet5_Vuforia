using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;

public class WishListPage : Page
{
    public WishListPage(VisualTreeAsset visualTreeAsset) : base(visualTreeAsset)
    {
    }

    public static WishListPage CreateInstance(VisualTreeAsset visualTreeAsset)
    {
        return new WishListPage(visualTreeAsset);
    }

    public void Initialize(PagesManager pagesManager)
    {
        GenerateWishListItems(pagesManager);
        FooterController.InitializeFooter(Root, pagesManager);

        // 查找 ShoppingCartTopBar
        var shoppingCartTopBar = Root.Q<VisualElement>("ShoppingCartTopBar");
        if (shoppingCartTopBar != null)
        {
            Debug.Log("ShoppingCartTopBar found in Initialize method.");
            shoppingCartTopBar.RegisterCallback<ClickEvent>(evt =>
            {
                Debug.Log("ShoppingCartTopBar clicked.");
                pagesManager.ShowPage("ShoppingCartPage");
            });
        }
        else
        {
            Debug.LogError("ShoppingCartTopBar not found in Initialize method.");
        }
    }

    private void GenerateWishListItems(PagesManager pagesManager)
    {
        var wishListContainer = Root.Q<VisualElement>("WishListContainer");
        
        if (wishListContainer == null)
        {
            Debug.LogError("WishListContainer not found in WishListPage.");
            return;
        }

        wishListContainer.Clear();

        if (!pagesManager.pageAssets.TryGetValue("WishListCartTemplate", out var wishListCartTemplate))
        {
            Debug.LogError("WishListCartTemplate not found in pageAssets.");
            return;
        }

        var wishListItems = WishListManager.Instance.GetWishListItems();
        
        foreach (var item in wishListItems)
        {
            var itemElement = wishListCartTemplate.CloneTree();

            var imgCart = itemElement.Q<VisualElement>("ImgCart");
            if (imgCart != null && item.icon != null)
            {
                imgCart.style.backgroundImage = new StyleBackground(item.icon);
            }

            var cartTitle = itemElement.Q<Label>("CartTitle");
            if (cartTitle != null)
            {
                cartTitle.text = item.itemName;
            }

            var description = itemElement.Q<Label>("Description");
            if (description != null)
            {
                description.text = item.description;
            }

            var price = itemElement.Q<Label>("Price");
            if (price != null)
            {
                price.text = $"${item.price}";
            }

            var deleteButton = itemElement.Q<Button>("DeleteButton");
            if (deleteButton != null)
            {
                deleteButton.clicked += () => RemoveWishListItem(item, itemElement, wishListContainer);
            }

            var addButton = itemElement.Q<VisualElement>("AddButton");
            if (addButton != null)
            {
                addButton.RegisterCallback<ClickEvent>(evt => AddToCart(item));
            }

            wishListContainer.Add(itemElement);
        }
    }

    private void AddToCart(FurnitureSO item)
    {
        ShoppingCartManager.Instance.AddToCart(item);
        Debug.Log($"{item.itemName} added to Shopping Cart.");
    }

    private void RemoveWishListItem(FurnitureSO item, VisualElement itemElement, VisualElement wishListContainer)
    {
        WishListManager.Instance.RemoveFromWishList(item);
        wishListContainer.Remove(itemElement);
    }
}