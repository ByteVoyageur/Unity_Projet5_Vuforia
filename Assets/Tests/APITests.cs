using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;


public class APITests
{
    private const string apiUrl = "https://xiaosong.fr/decomaison/api/user_api.php/login";

    [UnityTest]
    public IEnumerator LoginUserAPITest()
    {
        var jsonBody = JsonConvert.SerializeObject(new {
            action = "login",
            userName = "Bob",
            password = "abc123"
        });

        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody));
            www.uploadHandler.contentType = "application/json";
            www.downloadHandler = new DownloadHandlerBuffer();
            
            www.SetRequestHeader("Accept", "application/json");

            yield return www.SendWebRequest();

            Debug.Log("Raw Response: " + www.downloadHandler.text);

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.GetResponseHeader("Content-Type").Contains("application/json"))
                {
                    var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(www.downloadHandler.text);
                    Assert.IsTrue(responseData.ContainsKey("message"), "Login successful message missing in response");
                    Assert.AreEqual("Login successful", responseData["message"], "Unexpected message in response");
                }
                else
                {
                    Debug.LogError("Unexpected response type: " + www.GetResponseHeader("Content-Type"));
                    Assert.Fail("Unexpected response type, expected JSON.");
                }
            }
            else
            {
                Debug.LogError("Login API request failed: " + www.error);
                Assert.Fail("Login API request failed: " + www.error);
            }
        }
    }

    [UnityTest]
    public IEnumerator GetProductAPITest()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            www.SetRequestHeader("Accept", "application/json");

            yield return www.SendWebRequest();

            Debug.Log("Raw Response: " + www.downloadHandler.text);

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.GetResponseHeader("Content-Type").Contains("application/json"))
                {
                    var responseData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(www.downloadHandler.text);

                    var firstProduct = responseData[0];
                    Assert.AreEqual("2", firstProduct["product_id"].ToString(), "Product ID mismatch");
                    Assert.AreEqual("Chair leather", firstProduct["name"].ToString(), "Product name mismatch");
                    Assert.AreEqual("4", firstProduct["category_id"].ToString(), "Category ID mismatch");
                    Assert.AreEqual("https://xiaosong.fr/decomaison/img/furniture-kitchen-chair.png", firstProduct["image_url"].ToString(), "Image URL mismatch");
                    Assert.AreEqual("Leather chair comfort", firstProduct["description"].ToString(), "Description mismatch");
                    Assert.AreEqual("100", firstProduct["price"].ToString(), "Price mismatch");
                    Assert.AreEqual("ChairLeatherPrefab", firstProduct["prefabName"].ToString(), "Prefab name mismatch");
                }
                else
                {
                    Debug.LogError("Unexpected response type: " + www.GetResponseHeader("Content-Type"));
                    Assert.Fail("Unexpected response type, expected JSON.");
                }
            }
            else
            {
                Debug.LogError("API request failed: " + www.error);
                Assert.Fail("API request failed: " + www.error);
            }
        }
    }

    private const string wishlistApiUrl = "https://xiaosong.fr/decomaison/api/user_api.php?action=get_wishlist&user_id=4";

    [UnityTest]
    public IEnumerator GetWishlistAPITest()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(wishlistApiUrl))
        {
            www.SetRequestHeader("Accept", "application/json");

            yield return www.SendWebRequest();

            Debug.Log("Raw Response: " + www.downloadHandler.text);

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.GetResponseHeader("Content-Type").Contains("application/json"))
                {
                    var responseData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(www.downloadHandler.text);

                    var wishlistItem = responseData.Find(item => item["product_id"].ToString() == "5");
                    Assert.IsNotNull(wishlistItem, "Wishlist item not found.");

                    Assert.AreEqual("BedExcutivePrefab", wishlistItem["prefabName"].ToString(), "Prefab name mismatch.");
                }
                else
                {
                    Debug.LogError("Unexpected response type: " + www.GetResponseHeader("Content-Type"));
                    Assert.Fail("Unexpected response type, expected JSON.");
                }
            }
            else
            {
                Debug.LogError("Wishlist API request failed: " + www.error);
                Assert.Fail("Wishlist API request failed: " + www.error);
            }
        }
    }

}
