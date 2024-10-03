using UnityEngine;
using System.Collections.Generic;

public class UserManager : MonoBehaviour {
    private static UserManager _instance;
    public static UserManager Instance {
        get {
            if (_instance == null) {
                _instance = new GameObject("UserManager").AddComponent<UserManager>();
            }
            return _instance;
        }
    }

    public bool IsLoggedIn { get; private set; }
    public bool IsAdmin { get; private set; }
    public int UserId { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public List<string> WishList { get; private set; }
    public List<string> ShoppingCart { get; private set; }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeUserData();
        } else if (_instance != this) {
            Destroy(gameObject);
        }
    }

    private void InitializeUserData() {
        IsLoggedIn = false;
        IsAdmin = false;
        UserId = -1;  // Default user ID for guest
        Username = "";
        Email = "";
        WishList = new List<string>();
        ShoppingCart = new List<string>();
    }

    public void LogIn(int userId, bool isAdmin, string username, string email) {
        IsLoggedIn = true;
        IsAdmin = isAdmin;
        UserId = userId;
        Username = username;
        Email = email;
    }

    public void LogOut() {
        IsLoggedIn = false;
        IsAdmin = false;
        UserId = -1;
        Username = "";
        Email = "";
        WishList.Clear();
        ShoppingCart.Clear();
    }

    public void AddToWishList(string item) {
        if (!WishList.Contains(item)) {
            WishList.Add(item);
        }
    }

    public void RemoveFromWishList(string item) {
        if (WishList.Contains(item)) {
            WishList.Remove(item);
        }
    }

    public void AddToShoppingCart(string item) {
        if (!ShoppingCart.Contains(item)) {
            ShoppingCart.Add(item);
        }
    }

    public void RemoveFromShoppingCart(string item) {
        if (ShoppingCart.Contains(item)) {
            ShoppingCart.Remove(item);
        }
    }
}