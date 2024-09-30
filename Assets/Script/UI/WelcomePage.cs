using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UIElements;

public class WelcomePage : Page
{
    private MonoBehaviour _monoBehaviour;

    public WelcomePage(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour) : base(visualTreeAsset)
    {
        _monoBehaviour = monoBehaviour;
    }

    public void Initialize(PagesManager pagesManager)
    {
        // Use the MonoBehaviour instance to start the coroutine
        _monoBehaviour.StartCoroutine(GetCategories());
    }

    [System.Serializable]
    public class Category
    {
        public int category_id;
        public string name; 
        public string url;  
    }

    [System.Serializable]
    public class CategoryList
    {
        public List<Category> categories;
    }

    // Use HTTPS URL
    private string apiUrl = "https://xiaosong.fr/decomaison/api/user_api.php?categories";

    IEnumerator GetCategories()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            // Assign a certificate handler to handle SSL certificate validation
            www.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("HTTP error: " + www.error); // Log detailed error
                Debug.LogError("Response code: " + www.responseCode); // Log response code
                Debug.LogError("Response text: " + www.downloadHandler.text); // Log response text
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Received response: " + jsonResponse); // Log the successful response

                // 将jsonResponse反序列化为List<Category>
                List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(jsonResponse);

                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        Debug.Log($"Category ID: {category.category_id}, Name: {category.name}, URL: {category.url}");
                        // Update UI here
                    }
                }
                else
                {
                    Debug.LogError("Failed to deserialize categories.");
                }
            }
        }
    }

    public static WelcomePage CreateInstance(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour)
    {
        return new WelcomePage(visualTreeAsset, monoBehaviour);
    }

    // This class accepts any SSL certificate - replace this with proper validation for production
    private class AcceptAllCertificatesSignedWithASpecificKeyPublicKey : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true; // For now, accept all certificates
        }
    }
}