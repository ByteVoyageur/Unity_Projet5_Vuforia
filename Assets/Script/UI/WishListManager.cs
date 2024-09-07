using System.Collections.Generic;
using UnityEngine;

public class WishListManager : MonoBehaviour
{
    private static WishListManager _instance;
    public static WishListManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("WishListManager").AddComponent<WishListManager>();
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    private List<FurnitureSO> wishList = new List<FurnitureSO>();

    public void AddToWishList(FurnitureSO item)
    {
        if (!wishList.Contains(item))
        {
            wishList.Add(item);
            Debug.Log($"{item.itemName} added to Wish List.");
            PrintWishList();
        }
    }

    public void RemoveFromWishList(FurnitureSO item)
    {
        if (wishList.Contains(item))
        {
            wishList.Remove(item);
            Debug.Log($"{item.itemName} removed from Wish List.");
            if (wishList.Count == 0)
            {
                DeleteWishList();
            }
            PrintWishList();
        }
    }

    private void DeleteWishList()
    {
        wishList = new List<FurnitureSO>();
        Debug.Log("Wish List deleted.");
    }

    public bool IsInWishList(FurnitureSO item)
    {
        return wishList.Contains(item);
    }

    public void PrintWishList()
    {
        if (wishList.Count == 0)
        {
            Debug.Log("Wish List is empty.");
            return;
        }

        Debug.Log("Current Wish List:");
        foreach (var item in wishList)
        {
            Debug.Log(" - " + item.itemName);
        }
    }

    public List<FurnitureSO> GetWishListItems()
    {
        return wishList;
    }
}