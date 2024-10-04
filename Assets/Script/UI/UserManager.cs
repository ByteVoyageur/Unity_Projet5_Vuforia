using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class UserManager : MonoBehaviour
{
    private static UserManager _instance;
    public static UserManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UserManager>();
                if (_instance == null)
                {
                    GameObject userManagerObject = new GameObject("UserManager");
                    _instance = userManagerObject.AddComponent<UserManager>();
                }
            }
            return _instance;
        }
    }

    public bool IsLoggedIn { get; private set; }
    public bool IsAdmin { get; private set; }
    public int UserId { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }

    public List<WishListManager.Item> WishList { get; private set; }
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
        UserId = -1;  
        Username = "";
        Email = "";
        WishList = new List<WishListManager.Item>();  
        ShoppingCart = new List<string>();
    }

    public void LogIn(int userId, bool isAdmin, string username, string email)
    {
        IsLoggedIn = true;
        IsAdmin = isAdmin;
        UserId = userId;
        Username = username;
        Email = email;
    }

    public void LogOut()
    {
        IsLoggedIn = false;
        IsAdmin = false;
        UserId = -1;
        Username = "";
        Email = "";
        WishList.Clear();
        ShoppingCart.Clear();

        if (PagesManager.Instance != null)
        {
            PagesManager.Instance.ClearPageCache();
        }
    }

    public IEnumerator FetchWishList()
    {
        string apiUrl = $"https://xiaosong.fr/decomaison/api/user_api.php?action=get_wishlist&user_id={UserId}";

        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                List<WishListManager.Item> wishListData = JsonConvert.DeserializeObject<List<WishListManager.Item>>(www.downloadHandler.text);

                WishList.Clear();
                WishList.AddRange(wishListData);

                Debug.Log("WishList updated successfully.");
            }
            else
            {
                Debug.LogError("Failed to fetch wish list: " + www.error);
            }
        }
    }

    public IEnumerator SyncUserState()
    {
        string apiUrl = $"https://xiaosong.fr/decomaison/api/user_api.php?user_id={UserId}&action=get_user_status";

        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var userData = JsonConvert.DeserializeObject<Dictionary<string, object>>(www.downloadHandler.text);

                int userId = Convert.ToInt32(userData["user_id"]);
                bool isAdmin = userData["is_admin"].ToString() == "1";
                string userName = userData["userName"].ToString();
                string email = userData["email"].ToString();

                LogIn(userId, isAdmin, userName, email);

                yield return StartCoroutine(FetchWishList());
            }
            else
            {
                Debug.LogError("Failed to sync user state: " + www.error);
            }
        }
    }

    public void AddToWishList(WishListManager.Item newItem)
    {
        if (!WishList.Exists(p => p.product_id == newItem.product_id))
        {
            WishList.Add(newItem);
        }
    }

    public void RemoveFromWishList(WishListManager.Item itemToRemove)
    {
        WishList.RemoveAll(p => p.product_id == itemToRemove.product_id);
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
