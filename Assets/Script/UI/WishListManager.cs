using System;
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

    public event Action<FurnitureSO> OnItemAddedToWishList;
    public event Action<FurnitureSO> OnItemRemovedFromWishList;

    // add to wish list
    public void AddToWishList(FurnitureSO item)
    {
        if (!wishList.Contains(item))
        {
            wishList.Add(item);
            Debug.Log($"{item.itemName} added to Wish List.");
            PrintWishList();
            OnItemAddedToWishList?.Invoke(item);
        }
    }

    // remove from wish list
    public void RemoveFromWishList(FurnitureSO item)
    {
        if (wishList.Contains(item))
        {
            wishList.Remove(item);
            Debug.Log($"{item.itemName} removed from Wish List.");
            PrintWishList();
            OnItemRemovedFromWishList?.Invoke(item);
        }
    }

    // check if in wish list
    public bool IsInWishList(FurnitureSO item)
    {
        return wishList.Contains(item);
    }

    // get list of wish list
    public List<FurnitureSO> GetWishListItems()
    {
        return wishList;
    }

    // print current wish list
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
}