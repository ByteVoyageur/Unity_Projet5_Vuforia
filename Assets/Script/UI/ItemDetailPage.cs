using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ItemDetailPage : Page
{
    private string arSceneName = "SampleScene";
    private PagesManager pagesManager;
    private FurnitureSO currentItemData;

    public ItemDetailPage(VisualTreeAsset visualTreeAsset) : base(visualTreeAsset)
    {
    }

    public static ItemDetailPage CreateInstance(VisualTreeAsset visualTreeAsset)
    {
        return new ItemDetailPage(visualTreeAsset);
    }

    public void Initialize(FurnitureSO itemData, PagesManager manager)
    {
        currentItemData = itemData;
        pagesManager = manager;

        // Update UI elements with itemData values
        var imgItem = Root.Q<VisualElement>("ImgItem");
        if (imgItem != null && itemData.icon != null)
        {
            imgItem.style.backgroundImage = new StyleBackground(itemData.icon);
        }

        var nameItem = Root.Q<Label>("NameItem");
        if (nameItem != null)
        {
            nameItem.text = itemData.itemName;
        }

        var descriptionItem = Root.Q<Label>("Description");
        if (descriptionItem != null)
        {
            descriptionItem.text = itemData.description;
        }

        var priceEuros = Root.Q<Label>("PriceEuros");
        if (priceEuros != null)
        {
            int wholePrice = Mathf.FloorToInt(itemData.price);
            priceEuros.text = wholePrice.ToString();
        }

        var priceCentimes = Root.Q<Label>("PriceCentimes");
        if (priceCentimes != null)
        {
            int centimes = Mathf.FloorToInt((itemData.price - Mathf.Floor(itemData.price)) * 100);
            priceCentimes.text = "," + centimes.ToString("D2") + "€";
        }

        var viewInARButton = Root.Q<Button>("ViewInARButton");
        if (viewInARButton != null)
        {
            viewInARButton.clicked += OnViewInARButtonClick;
        }
        else
        {
            Debug.LogError("ViewInARButton element not found on ItemDetailPage.");
        }

        // Handle HeartForWishList button click
        var heartForWishList = Root.Q<VisualElement>("HeartForWishList");
        if (heartForWishList != null)
        {
            heartForWishList.RegisterCallback<ClickEvent>(evt => {
                HandleWishListClick(itemData, heartForWishList);
            });
            UpdateHeartVisualState(heartForWishList, WishListManager.Instance.IsInWishList(itemData));
        }
        else
        {
            Debug.LogError("HeartForWishList element not found on ItemDetailPage.");
        }

        // Listen for wishlist changes
        WishListManager.Instance.OnItemAddedToWishList += (item) => {
            if (item == currentItemData)
            {
                UpdateHeartVisualState(heartForWishList, true);
            }
        };

        WishListManager.Instance.OnItemRemovedFromWishList += (item) => {
            if (item == currentItemData)
            {
                UpdateHeartVisualState(heartForWishList, false);
            }
        };

        // Handle IconBack button click
        var iconBack = Root.Q<VisualElement>("IconBack");
        if (iconBack != null)
        {
            iconBack.RegisterCallback<ClickEvent>(evt => {
                OnIconBackClick();
            });
        }
        else
        {
            Debug.LogError("IconBack element not found on ItemDetailPage.");
        }
    }

    private void OnViewInARButtonClick()
    {
        SceneManager.LoadScene(arSceneName, LoadSceneMode.Single);
    }

    private void OnIconBackClick()
    {
        var categoryData = pagesManager.GetCurrentCategory();
        if (pagesManager != null)
        {
            pagesManager.ShowPage("CategoryPage", categoryData);
        }
        else
        {
            Debug.LogError("PagesManager instance is null.");
        }
    }

    private void HandleWishListClick(FurnitureSO itemData, VisualElement heartForWishList)
    {
        if (WishListManager.Instance.IsInWishList(itemData))
        {
            WishListManager.Instance.RemoveFromWishList(itemData);
        }
        else
        {
            WishListManager.Instance.AddToWishList(itemData);
        }
    }

    private void UpdateHeartVisualState(VisualElement heartForWishList, bool isInWishList)
    {
        if (isInWishList)
        {
            heartForWishList.AddToClassList("in-wish-list");
            Debug.Log("Added to wish list.");
        }
        else
        {
            heartForWishList.RemoveFromClassList("in-wish-list");
            Debug.Log("Removed from wish list.");
        }
    }
}