using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Newtonsoft.Json;

public class WelcomePage : Page
{
    private MonoBehaviour _monoBehaviour;

    public WelcomePage(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour) : base(visualTreeAsset)
    {
        _monoBehaviour = monoBehaviour;
    }

    public static WelcomePage CreateInstance(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour)
    {
        return new WelcomePage(visualTreeAsset, monoBehaviour);
    }

    public void Initialize(PagesManager pagesManager)
    {
        _monoBehaviour.StartCoroutine(GetCategoriesFromAPI(pagesManager));

        FooterController.InitializeFooter(Root, pagesManager);
    }

    [System.Serializable]
    public class Category
    {
        public int category_id;
        public string name;  
        public string url;   
    }

    private string apiUrl = "https://xiaosong.fr/decomaison/api/user_api.php?categories";

    private IEnumerator GetCategoriesFromAPI(PagesManager pagesManager)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("HTTP error: " + www.error);
                Debug.LogError("Response code: " + www.responseCode);
                Debug.LogError("Response text: " + www.downloadHandler.text);
                yield break;
            }

            string jsonResponse = www.downloadHandler.text;
            Debug.Log("Received response: " + jsonResponse);

            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(jsonResponse);
            GenerateCategories(categories, pagesManager);
        }
    }

    private void GenerateCategories(List<Category> categories, PagesManager pagesManager)
    {
        var categoryList = Root.Q<ScrollView>("CategorysScrollList");

        if (!pagesManager.pageAssets.TryGetValue("CategoryCartTemplate", out var categoryCartTemplate))
        {
            Debug.LogError("CategoryCartTemplate not found in pageAssets.");
            return;
        }

        foreach (var categoryData in categories)
        {
            var categoryName = categoryData.name;
            string imageUrl = categoryData.url;

            if (string.IsNullOrEmpty(categoryName))
            {
                Debug.LogError("Category name is missing.");
                continue;
            }

            var categoryCardInstance = categoryCartTemplate.CloneTree();

            var categoryImg = categoryCardInstance.Q<VisualElement>("CategoryCartImage");
            var categoryTitle = categoryCardInstance.Q<Label>("CategoryTitle");

            if (categoryTitle != null)
            {
                categoryTitle.text = categoryName;
            }

            _monoBehaviour.StartCoroutine(LoadImageFromURL(imageUrl, (texture) =>
            {
                if (categoryImg != null && texture != null)
                {
                    categoryImg.style.backgroundImage = new StyleBackground(texture);
                }
            }));

            categoryCardInstance.RegisterCallback<ClickEvent>(evt =>
            {
                Debug.Log($"Category {categoryName} clicked.");
                pagesManager.ShowCategoryPage(categoryName);
            });

            categoryList.Add(categoryCardInstance);
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