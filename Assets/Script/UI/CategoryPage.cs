using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Newtonsoft.Json;

public class CategoryPage : Page
{
    private MonoBehaviour _monoBehaviour;
    private string apiUrl = "https://xiaosong.fr/decomaison/api/user_api.php?products&category_id=";

    public CategoryPage(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour) : base(visualTreeAsset)
    {
        _monoBehaviour = monoBehaviour;
    }

    public static CategoryPage CreateInstance(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour)
    {
        return new CategoryPage(visualTreeAsset, monoBehaviour);
    }

    public void Initialize(PagesManager pagesManager, int categoryId)
    {
        _monoBehaviour.StartCoroutine(GetItemsFromAPI(pagesManager, categoryId));
        FooterController.InitializeFooter(Root, pagesManager);
    }

    [System.Serializable]
    public class Item
    {
        public int product_id;
        public string name;
        public int category_id;
        public string image_url;
        public string description;
        public float price;
    }

    private IEnumerator GetItemsFromAPI(PagesManager pagesManager, int categoryId)
    {
        string completeApiUrl = $"{apiUrl}{categoryId}";
        Debug.Log($"Request URL: {completeApiUrl}");

        using (UnityWebRequest www = UnityWebRequest.Get(completeApiUrl))
        {
            // Accept all certificates for HTTPS request
            www.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"HTTP Error: {www.error}");
                Debug.LogError($"Response Code: {www.responseCode}");
                Debug.LogError($"Response Text: {www.downloadHandler.text}");
                yield break;
            }

            string jsonResponse = www.downloadHandler.text;
            Debug.Log($"Raw Response: {jsonResponse}");

            if (jsonResponse.StartsWith("<") || jsonResponse.Contains("error"))
            {
                Debug.LogError($"Received an error HTML page or error message: {jsonResponse}");
                yield break;
            }

            try
            {
                List<Item> items = JsonConvert.DeserializeObject<List<Item>>(jsonResponse);
                GenerateItems(pagesManager, items);
            }
            catch (JsonException jsonEx)
            {
                Debug.LogError($"JSON Parse Error: {jsonEx.Message}");
                Debug.LogError($"Response was: {jsonResponse}");
            }
        }
    }

    private void GenerateItems(PagesManager pagesManager, List<Item> items)
    {
        var itemsContainer = Root.Q<ScrollView>("CategoryItemsScrollContainer");
        if (!pagesManager.pageAssets.TryGetValue("ItemCartTemplate", out var itemTemplate))
        {
            Debug.LogError("ItemTemplate not found in pageAssets.");
            return;
        }

        itemsContainer.Clear();
        foreach (var item in items)
        {
            var itemElement = itemTemplate.CloneTree();
            var itemImg = itemElement.Q<VisualElement>("Image");

            // Load each item's image
            _monoBehaviour.StartCoroutine(LoadImageFromURL(item.image_url, (texture) =>
            {
                if (itemImg != null && texture != null)
                {
                    itemImg.style.backgroundImage = new StyleBackground(texture);
                }
            }));

            var itemTitle = itemElement.Q<Label>("TitleCart");
            itemTitle.text = item.name;
            var itemDescription = itemElement.Q<Label>("DescriptionCart");
            itemDescription.text = item.description;
            var itemPrice = itemElement.Q<Label>("Price");
            itemPrice.text = $"${item.price}";

            // Register click event for each item card
            itemElement.RegisterCallback<ClickEvent>(evt =>
            {
                Debug.Log($"Item {item.name} clicked.");
                // Directly pass the item object to show details
                //pagesManager.ShowItemDetailPage(item);
            });

            itemsContainer.Add(itemElement);
        }
    }

    private IEnumerator LoadImageFromURL(string imageUrl, System.Action<Texture2D> onSuccess)
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
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
            }
            else
            {
                Debug.LogError("Failed to load texture from " + imageUrl);
                onSuccess?.Invoke(null);
            }
        }
    }

    private class AcceptAllCertificatesSignedWithASpecificKeyPublicKey : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}