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
        FooterController.InitializeFooter(Root, pagesManager); // 确保FooterTemplate的初始化在AddTotalPriceAndPayButton之后
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

        AddTotalPriceAndPayButton(totalPrice);
    }

    private void AddTotalPriceAndPayButton(float totalPrice)
    {
        var parentContainer = Root.Q<VisualElement>("Background");

        if (parentContainer == null)
        {
            Debug.LogError("Background element not found in ShoppingCartPage.");
            return;
        }

        var shoppingCartListContainer = Root.Q<VisualElement>("ShoppingCartListContainer");
        var footerTemplate = Root.Q<TemplateContainer>("FooterTemplate");

        if (shoppingCartListContainer == null || footerTemplate == null)
        {
            Debug.LogError("ShoppingCartListContainer or FooterTemplate not found in ShoppingCartPage.");
            return;
        }

        var existingTotalPricePayContainer = parentContainer.Q<VisualElement>("TotalPricePayContainer");
        if (existingTotalPricePayContainer != null)
        {
            parentContainer.Remove(existingTotalPricePayContainer);
        }

        var totalPricePayContainer = new VisualElement
        {
            name = "TotalPricePayContainer"
        };
        totalPricePayContainer.AddToClassList("total-price-pay-container");

        var totalPriceLabel = new Label($"Total: ${totalPrice}")
        {
            name = "TotalPrice"
        };
        totalPriceLabel.AddToClassList("total-price-label");
        totalPriceLabel.style.fontSize = 35;
        totalPriceLabel.style.unityFontStyleAndWeight = FontStyle.Bold;

        var payButton = new Button(() => { Debug.Log("Pay button clicked"); })
        {
            text = "Pay"
        };
        payButton.AddToClassList("pay-button");
        payButton.style.color = new StyleColor(new Color(1f, 1f, 1f, 1f));
        payButton.style.fontSize = 30;
        totalPricePayContainer.Add(totalPriceLabel);
        totalPricePayContainer.Add(payButton);

        parentContainer.Insert(parentContainer.IndexOf(footerTemplate), totalPricePayContainer);
    }

    private void RemoveCartItem(FurnitureSO item, VisualElement itemElement, VisualElement cartContainer)
    {
        ShoppingCartManager.Instance.RemoveFromCart(item);
        cartContainer.Remove(itemElement);
        UpdateTotalPrice();
    }

    private void UpdateTotalPrice()
    {
        var cartItems = ShoppingCartManager.Instance.GetCartItems();
        float totalPrice = cartItems.Sum(i => i.price);

        var totalPriceLabel = Root.Q<Label>("TotalPrice");
        if (totalPriceLabel != null)
        {
            totalPriceLabel.text = $"Total: ${totalPrice}";
        }
    }
}