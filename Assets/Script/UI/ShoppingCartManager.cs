using System;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCartManager : MonoBehaviour
{
    private static ShoppingCartManager _instance;
    public static ShoppingCartManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("ShoppingCartManager").AddComponent<ShoppingCartManager>();
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    private List<FurnitureSO> shoppingCart = new List<FurnitureSO>();

    public event Action<FurnitureSO> OnItemAddedToCart;
    public event Action<FurnitureSO> OnItemRemovedFromCart;

    // Add to shopping cart
    public void AddToCart(FurnitureSO item)
    {
        if (!shoppingCart.Contains(item))
        {
            shoppingCart.Add(item);
            Debug.Log($"{item.itemName} added to Shopping Cart.");
            PrintShoppingCart();
            OnItemAddedToCart?.Invoke(item);
        }
    }

    // Remove from shopping cart
    public void RemoveFromCart(FurnitureSO item)
    {
        if (shoppingCart.Contains(item))
        {
            shoppingCart.Remove(item);
            Debug.Log($"{item.itemName} removed from Shopping Cart.");
            PrintShoppingCart();
            OnItemRemovedFromCart?.Invoke(item);
        }
    }

    // Check if in shopping cart
    public bool IsInCart(FurnitureSO item)
    {
        return shoppingCart.Contains(item);
    }

    // Get list of shopping cart items
    public List<FurnitureSO> GetCartItems()
    {
        return shoppingCart;
    }

    // Print current shopping cart
    public void PrintShoppingCart()
    {
        if (shoppingCart.Count == 0)
        {
            Debug.Log("Shopping Cart is empty.");
            return;
        }

        Debug.Log("Current ShoppingCart:");
        foreach (var item in shoppingCart)
        {
            Debug.Log(" - " + item.itemName);
        }
    }
}