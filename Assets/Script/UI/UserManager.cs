using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

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

public IEnumerator SyncUserState() {
    string apiUrl = $"https://xiaosong.fr/decomaison/api/user_api.php?user_id={UserId}&action=get_user_status";

    using (UnityWebRequest www = UnityWebRequest.Get(apiUrl)) {
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.Success) {
            var userData = JsonConvert.DeserializeObject<Dictionary<string, object>>(www.downloadHandler.text);
            
            int userId = Convert.ToInt32(userData["user_id"]);
            bool isAdmin = userData["is_admin"].ToString() == "1"; 
            string userName = userData["userName"].ToString();
            string email = userData["email"].ToString();

            LogIn(userId, isAdmin, userName, email);
        } else {
            Debug.LogError("Failed to sync user state: " + www.error);
        }
    }
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