using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class ShoppingCartPage : Page
{
    public ShoppingCartPage(VisualTreeAsset visualTreeAsset) : base(visualTreeAsset)
    {
    }

    public static ShoppingCartPage CreateInstance(VisualTreeAsset visualTreeAsset)
    {
        return new ShoppingCartPage(visualTreeAsset);
    }

    public void Initialize(PagesManager pagesManager)
    {
        GenerateShoppingCartItems(pagesManager);
        FooterController.InitializeFooter(Root, pagesManager);
    }

    private void GenerateShoppingCartItems(PagesManager pagesManager)
    {
        var shoppingCartContainer = Root.Q<VisualElement>("ShoppingCartList");

        if (shoppingCartContainer == null)
        {
            Debug.LogError("ShoppingCartList not found in ShoppingCartPage.");
            return;
        }

        shoppingCartContainer.Clear();

        if (!pagesManager.pageAssets.TryGetValue("WishListCartTemplate", out var cartTemplate))
        {
            Debug.LogError("WishListCartTemplate not found in pageAssets.");
            return;
        }

        var cartItems = ShoppingCartManager.Instance.GetCartItems();
        float totalPrice = 0;

        foreach (var item in cartItems)
        {
            var itemElement = cartTemplate.CloneTree();
        
        // Remove "Add to cart" button if exists
        var addButton = itemElement.Q<VisualElement>("AddButton");
        if (addButton != null)
        {
            addButton.RemoveFromHierarchy();
        }

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
                totalPrice += item.price;
            }

            var deleteButton = itemElement.Q<Button>("DeleteButton");
            if (deleteButton != null)
            {
                deleteButton.clicked += () => RemoveCartItem(item, itemElement, shoppingCartContainer);
            }

            shoppingCartContainer.Add(itemElement);
        }

        var totalPriceLabel = Root.Q<Label>("TotalPrice");
        if (totalPriceLabel != null)
        {
            totalPriceLabel.text = $"Total: ${totalPrice}";
        }
    }

    private void RemoveCartItem(FurnitureSO item, VisualElement itemElement, VisualElement cartContainer)
    {
        ShoppingCartManager.Instance.RemoveFromCart(item);
        cartContainer.Remove(itemElement);
        UpdateTotalPrice();
    }

    private void UpdateTotalPrice()
    {
        var totalPriceLabel = Root.Q<Label>("TotalPrice");
        if (totalPriceLabel != null)
        {
            var totalPrice = ShoppingCartManager.Instance.GetCartItems().Sum(i => i.price);
            totalPriceLabel.text = $"Total: ${totalPrice}";
        }
    }
}
