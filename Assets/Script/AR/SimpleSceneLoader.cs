using UnityEngine;
using System.Collections.Generic;

public class SimpleSceneLoader : MonoBehaviour
{
    private void Start()
    {
        // Load wishlist items from the server
        LoadWishListItems();
    }

    private void LoadWishListItems()
    {
        // Fetch items from the WishListManager's fetched list
        var wishListItems = WishListManager.Instance.GetWishListItems();

        if (wishListItems == null || wishListItems.Count == 0)
        {
            Debug.LogError("No items in wishlist to load into SimpleScene.");
            return;
        }

        // Find ObjectManager in the scene
        var objectManager = FindObjectOfType<ObjectManager>();
        if (objectManager == null)
        {
            Debug.LogError("ObjectManager not found in SimpleScene.");
            return;
        }

        int count = 0;
        foreach (var item in wishListItems)
        {
            Debug.Log($"Attempting to load prefab for item {item.name}");
            // Assuming `prefab` and `icon` are part of the Item class or can be fetched separately.
            if (item.prefab != null)
            {
                Debug.Log($"Prefab found for item {item.name}, adding to ObjectManager");
                objectManager.AddObject(item.prefab, item.icon, count);
                count++;
            }
            else
            {
                Debug.LogError($"Prefab not assigned for item {item.name}");
            }
        }
    }
}