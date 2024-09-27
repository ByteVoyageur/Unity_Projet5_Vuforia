using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using System.Collections;

public class SignInPage : Page
{
    public SignInPage(VisualTreeAsset visualTreeAsset) : base(visualTreeAsset) { }

    public static SignInPage CreateInstance(VisualTreeAsset visualTreeAsset)
    {
        return new SignInPage(visualTreeAsset);
    }

    public void Initialize(PagesManager pagesManager, System.Func<IEnumerator,Coroutine>startCoroutine)
    {
        // Find ArrowToRight element and add click event to navigate back to HomePage
        var arrowToRight = Root.Q<VisualElement>("ArrowToRight");
        
        if (arrowToRight != null)
        {
            arrowToRight.RegisterCallback<ClickEvent>(evt =>
            {
                pagesManager.ShowPage("HomePage");
            });
        }
        else
        {
            Debug.LogError("ArrowToRight element not found on SignInPage.");
        }

        // Find login button and register click event
        var loginButton = Root.Q<Button>("LoginButton");
        if (loginButton != null)
        {
            loginButton.RegisterCallback<ClickEvent>(evt =>
            {
                string username = Root.Q<TextField>("EmailOrUserName").value;
                string password = Root.Q<TextField>("PassWord").value;
                StartCoroutine(LoginUser(username, password));
            });
        }
        else
        {
            Debug.LogError("Login button not found on SignInPage.");
        }

        // Find create account button and register click event
        var createAccountButton = Root.Q<Button>("CreateMyAccount");
        if (createAccountButton != null)
        {
            createAccountButton.RegisterCallback<ClickEvent>(evt =>
            {
                // For simplicity, let's assume you already have the necessary input fields
                string username = Root.Q<TextField>("EmailOrUserName").value;
                string password = Root.Q<TextField>("PassWord").value;
                string email = Root.Q<TextField>("EmailOrUserName").value;
                StartCoroutine(RegisterUser(username, password, email));
            });
        }
        else
        {
            Debug.LogError("Create account button not found on SignInPage.");
        }
    }

    private IEnumerator LoginUser(string username, string password)
    {
        var jsonBody = JsonConvert.SerializeObject(new
        {
            action = "login",
            username = username,
            password = password
        });

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm("https://xiaosong.fr/api/user_api.php/login", jsonBody))
        {
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody));
            www.uploadHandler.contentType = "application/json";
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Login successful: " + www.downloadHandler.text);
                // Optionally navigate to a different page
            }
            else
            {
                Debug.Log("Login failed: " + www.error);
            }
        }
    }

}