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
    if (itemsContainer == null)
    {
        Debug.LogError("itemsContainer is null");
        return;
    }

    if (!pagesManager.pageAssets.TryGetValue("ItemCartTemplate", out var itemTemplate))
    {
        Debug.LogError("ItemTemplate not found in pageAssets.");
        return;
    }

    itemsContainer.Clear();

    foreach (var item in items)
    {
        var itemElement = itemTemplate.CloneTree();
        if (itemElement == null)
        {
            Debug.LogError("Failed to clone itemElement from template.");
            continue;
        }

        var itemImg = itemElement.Q<VisualElement>("Image");
        if (itemImg != null)
        {
            Debug.Log($"Loading image for item: {item.name}, URL: {item.image_url}");
            _monoBehaviour.StartCoroutine(LoadImageFromURL(item.image_url, (texture) =>
            {
                if (texture != null)
                {
                    itemImg.style.backgroundImage = new StyleBackground(texture);
                    Debug.Log($"Successfully applied texture for item: {item.name}");
                }
                else
                {
                    Debug.LogError($"Failed to load or apply texture for item: {item.name}");
                }
            }));
        }
        else
        {
            Debug.LogError("Image element not found.");
        }

        var itemTitle = itemElement.Q<Label>("TitleCart");
        if (itemTitle != null)
        {
            itemTitle.text = item.name;
        }
        else
        {
            Debug.LogError("TitleCart not found.");
        }

        var itemDescription = itemElement.Q<Label>("DescriptionCart");
        if (itemDescription != null)
        {
            itemDescription.text = item.description;
        }
        else
        {
            Debug.LogError("DescriptionCart not found.");
        }

        var itemPrice = itemElement.Q<Label>("Price");
        if (itemPrice != null)
        {
            itemPrice.text = $"${item.price}";
        }
        else
        {
            Debug.LogError("Price element not found.");
        }

        itemElement.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log($"Item {item.name} clicked.");
            // pagesManager.ShowItemDetailPage(item);
        });

        itemsContainer.Add(itemElement);
    }
}

   private IEnumerator LoadImageFromURL(string imageUrl, System.Action<Texture2D> onSuccess)
{
    if (string.IsNullOrEmpty(imageUrl))
    {
        Debug.LogError("Image URL is null or empty.");
        onSuccess?.Invoke(null);
        yield break;
    }

    Debug.Log($"Loading image from URL: {imageUrl}");

    using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
    {
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Debug.Log($"Successfully loaded image from URL: {imageUrl}");
            onSuccess?.Invoke(texture);
        }
        else
        {
            Debug.LogError($"Failed to load texture from {imageUrl}, Error: {request.error}");
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