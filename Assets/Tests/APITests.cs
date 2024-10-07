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
}
