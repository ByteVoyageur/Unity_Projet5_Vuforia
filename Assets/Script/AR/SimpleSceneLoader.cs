using UnityEngine;
using System.Collections;

public class SimpleSceneLoader : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadWishListItemsCoroutine());
    }

    private IEnumerator LoadWishListItemsCoroutine()
    {
        bool isDataLoaded = false;
        WishListManager.Instance.FetchWishListItems(items =>
        {
            isDataLoaded = true;
        });

        while (!isDataLoaded)
        {
            yield return null;
        }

        var wishListItems = WishListManager.Instance.GetWishListItems();

        if (wishListItems == null || wishListItems.Count == 0)
        {
            Debug.LogError("No items in wishlist to load into SimpleScene.");
            yield break;
        }

        var objectManager = FindObjectOfType<ObjectManager>();
        if (objectManager == null)
        {
            Debug.LogError("ObjectManager not found in SimpleScene.");
            yield break;
        }

        int count = 0;
        foreach (var item in wishListItems)
        {
            if (item.prefab != null)
            {
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
