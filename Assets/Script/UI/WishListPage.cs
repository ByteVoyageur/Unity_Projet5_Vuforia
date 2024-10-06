using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;

public class WishListPage : Page
{
    private MonoBehaviour _monoBehaviour;
    private PagesManager pagesManager;


    public WishListPage(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour) : base(visualTreeAsset) 
    {
        _monoBehaviour = monoBehaviour;
    }

    public static WishListPage CreateInstance(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour)
    {
        return new WishListPage(visualTreeAsset, monoBehaviour);
    }

    public void Initialize(PagesManager pagesManager)
{
    Debug.Log("Initialize called");
    _monoBehaviour.StartCoroutine(InitializeCoroutine(pagesManager));
}

private IEnumerator InitializeCoroutine(PagesManager pagesManager)
{
    yield return _monoBehaviour.StartCoroutine(UserManager.Instance.SyncUserState());

    yield return _monoBehaviour.StartCoroutine(WishListManager.Instance.FetchWishListItemsCoroutine(wishListItems =>
    {
        if (wishListItems != null)
        {
            Debug.Log("WishList items fetched successfully");
            GenerateWishListItems(wishListItems, pagesManager);
        }
        else
        {
            Debug.LogError("Failed to fetch wish list items.");
        }
    }));

    FooterController.InitializeFooter(Root, pagesManager);

    var shoppingCartTopBar = Root.Q<VisualElement>("ShoppingCartTopBar");
    if (shoppingCartTopBar != null)
    {
        shoppingCartTopBar.RegisterCallback<ClickEvent>(evt =>
        {
            pagesManager.ShowPage("ShoppingCartPage");
        });
    }
    else
    {
        Debug.LogError("ShoppingCartTopBar not found in Initialize method.");
    }
}

    public override void OnNavigatedTo(PagesManager pagesManager)
    {
        this.pagesManager = pagesManager;
        RefreshWishListItems();
    }

    private void RefreshWishListItems()
    {
        _monoBehaviour.StartCoroutine(RefreshCoroutine());
    }

    private IEnumerator RefreshCoroutine()
    {
        yield return _monoBehaviour.StartCoroutine(WishListManager.Instance.FetchWishListItemsCoroutine(wishListItems =>
        {
            if (wishListItems != null)
            {
                Debug.Log("WishList items fetched successfully");
                GenerateWishListItems(wishListItems, pagesManager);
            }
            else
            {
                Debug.LogError("Failed to fetch wish list items.");
            }
        }));
    }


    private void GenerateWishListItems(List<WishListManager.Item> wishListItems, PagesManager pagesManager)
    {
        Debug.Log("GenerateWishListItems called");

        var wishListContainer = Root.Q<VisualElement>("WishListContainer");
        if (wishListContainer == null)
        {
            Debug.LogError("WishListContainer not found in WishListPage.");
            return;
        }

        wishListContainer.Clear();

        if (!pagesManager.pageAssets.TryGetValue("WishListCartTemplate", out var wishListCartTemplate))
        {
            Debug.LogError("WishListCartTemplate not found in pageAssets.");
            return;
        }

        wishListContainer.Clear(); 

        foreach (var item in wishListItems)
        {
            var itemElement = wishListCartTemplate.CloneTree();
            var imgCart = itemElement.Q<VisualElement>("ImgCart");
            if (imgCart != null)
            {
                var cachedTexture = WishListManager.Instance.GetCachedImage(item.image_url);
                if (cachedTexture != null)
                {
                    imgCart.style.backgroundImage = new StyleBackground(cachedTexture);
                }
                else
                {
                    Debug.Log("Starting coroutine to load image");
                    _monoBehaviour.StartCoroutine(LoadImage(item.image_url, texture =>
                    {
                        if (texture != null)
                        {
                            Debug.Log("Image loaded successfully");
                            WishListManager.Instance.CacheImage(item.image_url, texture);
                            imgCart.style.backgroundImage = new StyleBackground(texture);
                        }
                        else
                        {
                            Debug.LogError("Failed to load the image");
                        }
                    }));
                }
            }

            var cartTitle = itemElement.Q<Label>("CartTitle");
            if (cartTitle != null)
            {
                cartTitle.text = item.name;
            }

            var description = itemElement.Q<Label>("Description");
            if (description != null)
            {
                description.text = item.description;
            }

            var price = itemElement.Q<Label>("Price");
            if (price != null)
            {
                price.text = $"${item.price}";
            }

            var deleteButton = itemElement.Q<Button>("DeleteButton");
            if (deleteButton != null)
            {
                deleteButton.clicked += () => RemoveWishListItem(item, itemElement, wishListContainer);
            }

            var addButton = itemElement.Q<VisualElement>("AddButton");
            if (addButton != null)
            {
                addButton.RegisterCallback<ClickEvent>(evt => Debug.Log($"Clicked add button for {item.name}"));
            }

            wishListContainer.Add(itemElement);
        }
    }

    private IEnumerator LoadImage(string imageUrl, System.Action<Texture2D> onSuccess)
    {
        Debug.Log("LoadImage coroutine started");

        if (string.IsNullOrEmpty(imageUrl))
        {
            Debug.LogError("Image URL is null or empty.");
            onSuccess?.Invoke(null);
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                onSuccess?.Invoke(texture);
                Debug.Log("Image successfully loaded and passed to onSuccess callback");
            }
            else
            {
                Debug.LogError($"Error loading image: {request.error}");
                onSuccess?.Invoke(null);
            }
        }
    }

    private void RemoveWishListItem(WishListManager.Item item, VisualElement itemElement, VisualElement wishListContainer)
    {
        WishListManager.Instance.RemoveFromWishList(item);
        wishListContainer.Remove(itemElement);
    }
}