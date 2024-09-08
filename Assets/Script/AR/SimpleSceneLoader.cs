using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SimpleSceneLoader : MonoBehaviour
{
    private void Start()
    {
        WishListManager.Instance.PrintWishList();
        LoadWishListItems();
    }

    private void LoadWishListItems()
    {
        var wishListItems = WishListManager.Instance.GetWishListItems();

        if (wishListItems == null || wishListItems.Count == 0)
        {
            Debug.LogError("No items in wishlist to load into SimpleScene.");
            return;
        }

        var objectManager = FindObjectOfType<ObjectManager>();

        if (objectManager == null)
        {
            Debug.LogError("ObjectManager not found in SimpleScene.");
            return;
        }

        int count = 0;
        foreach (var item in wishListItems)
        {
            Debug.Log($"Attempting to load prefab for item {item.itemName}");

            if (item.prefab != null)
            {
                Debug.Log($"Prefab found for item {item.itemName}, adding to ObjectManager");
                objectManager.AddObject(item.prefab, item.icon, count);
                count++;  
            }
            else
            {
                Debug.LogError($"Prefab not assigned for item {item.itemName}");
            }
        }
    }
}