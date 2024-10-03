using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class WishListManager : MonoBehaviour
{
    private static WishListManager _instance;
    private string apiUrl = "https://xiaosong.fr/decomaison/api/user_api.php";

    public static WishListManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("WishListManager").AddComponent<WishListManager>();
            }
            return _instance;
        }
    }

    public event Action<Item> OnItemAddedToWishList;
    public event Action<Item> OnItemRemovedFromWishList;

    private List<Item> wishListItems = new List<Item>();
    private Dictionary<string, Texture2D> imageCache = new Dictionary<string, Texture2D>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Serializable]
    public class Item
    {
        public int product_id;
        public string name;
        public int category_id;
        public string image_url;
        public string description;
        public float price;
        public string prefabName;
        public GameObject prefab;
        public Texture2D icon;
    }

    public void AddToWishList(Item item)
    {
        StartCoroutine(AddToWishListCoroutine(item));
    }

    private IEnumerator AddToWishListCoroutine(Item item)
    {
        var requestPayload = new
        {
            action = "add_to_wishlist",
            user_id = UserManager.Instance.UserId,
            product_id = item.product_id
        };
        string jsonData = JsonConvert.SerializeObject(requestPayload);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Response: {www.downloadHandler.text}");
                item.prefab = LoadPrefab(item.prefabName);
                wishListItems.Add(item);
                OnItemAddedToWishList?.Invoke(item);
            }
            else
            {
                Debug.LogError($"Error adding to wishlist: {www.error}");
            }
        }
    }
    public void RemoveFromWishList(Item item)
    {
        StartCoroutine(RemoveFromWishListCoroutine(item));
    }

    private IEnumerator RemoveFromWishListCoroutine(Item item)
    {
    string url = $"{apiUrl}?action=remove_from_wishlist&user_id={UserManager.Instance.UserId}&product_id={item.product_id}";

    using (UnityWebRequest www = UnityWebRequest.Delete(url))
    {
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            wishListItems.Remove(item);
            OnItemRemovedFromWishList?.Invoke(item);
        }
        else
        {
            Debug.LogError($"Error removing from wishlist: {www.error}");
        }
    }
    }

    public void FetchWishListItems(Action<List<Item>> callback)
    {
        StartCoroutine(FetchWishListItemsCoroutine(callback));
    }

    private IEnumerator FetchWishListItemsCoroutine(Action<List<Item>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{apiUrl}?action=get_wishlist&user_id={UserManager.Instance.UserId}"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.Log($"JSON Response: {jsonResponse}");
                List<Item> items = JsonConvert.DeserializeObject<List<Item>>(jsonResponse);
                foreach (var item in items)
                {
                    item.prefab = LoadPrefab(item.prefabName);   
                    yield return StartCoroutine(DownloadImageCoroutine(item.image_url, texture =>
                    {
                        if (texture != null)
                        {
                            CacheImage(item.image_url, texture);
                        }
                    }));
                }
                wishListItems = items;
                callback?.Invoke(wishListItems);
            }
            else
            {
                Debug.LogError($"Error getting wishlist items: {www.error}");
                callback?.Invoke(null);
            }
        }
    }

    public IEnumerator DownloadImageCoroutine(string imageUrl, Action<Texture2D> onSuccess)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                onSuccess?.Invoke(texture);
            }
            else
            {
                Debug.LogError($"Error downloading image: {www.error}");
                onSuccess?.Invoke(null);
            }
        }
    }

    public bool IsInWishList(Item item)
    {
        return wishListItems.Exists(i => i.product_id == item.product_id);
    }

    public Texture2D GetCachedImage(string imageUrl)
    {
        if (imageCache.TryGetValue(imageUrl, out var texture))
        {
            return texture;
        }
        return null;
    }

    public void CacheImage(string imageUrl, Texture2D texture)
    {
        if (!imageCache.ContainsKey(imageUrl))
        {
            imageCache[imageUrl] = texture;
        }
    }

    public List<Item> GetWishListItems()
    {
        return wishListItems;
    }

    private GameObject LoadPrefab(string prefabName)
    {
        string path = $"Models/Furniture_FREE/Prefabs/{prefabName}";
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found at path: {path}");
        }
        return prefab;
    }
}