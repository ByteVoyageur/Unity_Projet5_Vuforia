using UnityEngine;
using System.Collections.Generic;

public class UserManager : MonoBehaviour
{
    // Singleton instance
    private static UserManager _instance;
    public static UserManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("UserManager").AddComponent<UserManager>();
            }
            return _instance;
        }
    }

    // User data
    public bool IsLoggedIn { get; private set; }
    public bool IsAdmin { get; private set; }
    public int UserId { get; private set; }  // Adding UserId
    public List<string> WishList { get; private set; }
    public List<string> ShoppingCart { get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeUserData();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeUserData()
    {
        IsLoggedIn = false;
        IsAdmin = false;
        UserId = 1;  // Default user ID for testing

        // Preserve wishlist for testing
        WishList = new List<string>{"item1", "item2"}; // Example item list
        // WishList = new List<string>();  // Original code
        ShoppingCart = new List<string>();
    }

    // Methods to manage user login status, wishlist, shopping cart
    public void LogIn(int userId, bool isAdmin)  // Now requires a userId
    {
        IsLoggedIn = true;
        IsAdmin = isAdmin;
        UserId = userId;
    }

    public void LogOut()
    {
        IsLoggedIn = false;
        IsAdmin = false;
        UserId = 1;  // Keep user ID for testing
        // UserId = -1;  // Original code
        // Preserve wishlist for testing
        // WishList.Clear();  // Original code
        ShoppingCart.Clear();
    }

    public void AddToWishList(string item)
    {
        if (!WishList.Contains(item))
        {
            WishList.Add(item);
        }
    }

    public void RemoveFromWishList(string item)
    {
        if (WishList.Contains(item))
        {
            WishList.Remove(item);
        }
    }

    public void AddToShoppingCart(string item)
    {
        if (!ShoppingCart.Contains(item))
        {
            ShoppingCart.Add(item);
        }
    }

    public void RemoveFromShoppingCart(string item)
    {
        if (ShoppingCart.Contains(item))
        {
            ShoppingCart.Remove(item);
        }
    }
}