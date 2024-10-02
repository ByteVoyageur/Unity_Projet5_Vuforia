using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ItemDetailPage : Page
{
    private MonoBehaviour _monoBehaviour;
    private string arSceneName = "SampleScene";
    private PagesManager pagesManager;
    private WishListManager.Item currentItemData;

    public ItemDetailPage(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour) : base(visualTreeAsset)
    {
        _monoBehaviour = monoBehaviour;
    }

    public static ItemDetailPage CreateInstance(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour)
    {
        return new ItemDetailPage(visualTreeAsset, monoBehaviour);
    }

    public void Initialize(WishListManager.Item itemData, PagesManager manager)
    {
        currentItemData = itemData;
        pagesManager = manager;

        SetItemImage(Root.Q<VisualElement>("ImgItem"), itemData.image_url);

        var nameItem = Root.Q<Label>("NameItem");
        nameItem.text = itemData.name;
        var descriptionItem = Root.Q<Label>("Description");
        descriptionItem.text = itemData.description;
        var priceEuros = Root.Q<Label>("PriceEuros");
        int wholePrice = Mathf.FloorToInt(itemData.price);
        priceEuros.text = wholePrice.ToString();
        var priceCentimes = Root.Q<Label>("PriceCentimes");
        int centimes = Mathf.FloorToInt((itemData.price - Mathf.Floor(itemData.price)) * 100);
        priceCentimes.text = "," + centimes.ToString("D2") + "€";

        var viewInARButton = Root.Q<Button>("ViewInARButton");
        viewInARButton.clicked += OnViewInARButtonClick;

        var heartForWishList = Root.Q<VisualElement>("HeartForWishList");
        var redHeart = Root.Q<VisualElement>("RedHeart");
        heartForWishList.RegisterCallback<ClickEvent>(evt => {
            HandleWishListClick(itemData, heartForWishList, redHeart);
        });

        UpdateHeartVisualState(heartForWishList, redHeart, WishListManager.Instance.IsInWishList(itemData));

        WishListManager.Instance.OnItemAddedToWishList -= OnItemAddedToWishList;
        WishListManager.Instance.OnItemRemovedFromWishList -= OnItemRemovedFromWishList;
        WishListManager.Instance.OnItemAddedToWishList += OnItemAddedToWishList;
        WishListManager.Instance.OnItemRemovedFromWishList += OnItemRemovedFromWishList;

        var iconBack = Root.Q<VisualElement>("IconBack");
        iconBack.RegisterCallback<ClickEvent>(evt => {
            OnIconBackClick();
        });
    }

    private void SetItemImage(VisualElement imgItem, string imageUrl)
    {
        var cachedTexture = WishListManager.Instance.GetCachedImage(imageUrl);
        if (cachedTexture != null)
        {
            imgItem.style.backgroundImage = new StyleBackground(cachedTexture);
        }
        else
        {
            _monoBehaviour.StartCoroutine(WishListManager.Instance.DownloadImageCoroutine(imageUrl, texture =>
            {
                if (texture != null)
                {
                    imgItem.style.backgroundImage = new StyleBackground(texture);
                }
            }));
        }
    }

    private void OnItemAddedToWishList(WishListManager.Item item)
    {
        if (item == currentItemData)
        {
            var heartForWishList = Root.Q<VisualElement>("HeartForWishList");
            var redHeart = Root.Q<VisualElement>("RedHeart");
            UpdateHeartVisualState(heartForWishList, redHeart, true);
        }
    }

    private void OnItemRemovedFromWishList(WishListManager.Item item)
    {
        if (item == currentItemData)
        {
            var heartForWishList = Root.Q<VisualElement>("HeartForWishList");
            var redHeart = Root.Q<VisualElement>("RedHeart");
            UpdateHeartVisualState(heartForWishList, redHeart, false);
        }
    }

    private void OnViewInARButtonClick()
    {
        SceneManager.LoadScene(arSceneName, LoadSceneMode.Single);
    }

    private void OnIconBackClick()
    {
        var categoryData = pagesManager.GetCurrentCategory();
        pagesManager.ShowPage("CategoryPage", categoryData);
    }

    private void HandleWishListClick(WishListManager.Item itemData, VisualElement heartForWishList, VisualElement redHeart)
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

    private void UpdateHeartVisualState(VisualElement heartForWishList, VisualElement redHeart, bool isInWishList)
    {
        if (isInWishList)
        {
            heartForWishList.style.visibility = Visibility.Hidden;
            redHeart.style.visibility = Visibility.Visible;
            Debug.Log("Added to wish list.");
        }
        else
        {
            heartForWishList.style.visibility = Visibility.Visible;
            redHeart.style.visibility = Visibility.Hidden;
            Debug.Log("Removed from wish list.");
        }
    }
}