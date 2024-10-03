using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

public class SignInPage : Page {
    private MonoBehaviour _monoBehaviour;

    public SignInPage(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour) : base(visualTreeAsset) {
        _monoBehaviour = monoBehaviour;
    }

    public static SignInPage CreateInstance(VisualTreeAsset visualTreeAsset, MonoBehaviour monoBehaviour) {
        return new SignInPage(visualTreeAsset, monoBehaviour);
    }

    public void Initialize(PagesManager pagesManager) {
        var arrowToRight = Root.Q<VisualElement>("ArrowToRight");
        var passwordField = Root.Q<TextField>("PassWord");
        if (passwordField != null) {
            passwordField.isPasswordField = true;
            }

        if (arrowToRight != null) {
            arrowToRight.RegisterCallback<ClickEvent>(evt => {
                pagesManager.ShowPage("HomePage");
            });
        } else {
            Debug.LogError("ArrowToRight element not found on SignInPage.");
        }

        var loginButton = Root.Q<Button>("LoginButton");
        if (loginButton != null) {
            loginButton.RegisterCallback<ClickEvent>(evt => {
                string userName = Root.Q<TextField>("EmailOrUserName").value;
                string password = Root.Q<TextField>("PassWord").value;
                _monoBehaviour.StartCoroutine(LoginUser(userName, password));
            });
        } else {
            Debug.LogError("Login button not found on SignInPage.");
        }

        var createAccountButton = Root.Q<Button>("CreateMyAccount");
        if (createAccountButton != null) {
            createAccountButton.RegisterCallback<ClickEvent>(evt => {
                string userName = Root.Q<TextField>("EmailOrUserName").value;
                string password = Root.Q<TextField>("PassWord").value;
                string email = Root.Q<TextField>("EmailOrUserName").value;
                _monoBehaviour.StartCoroutine(RegisterUser(userName, password, email));
            });
        } else {
            Debug.LogError("Create account button not found on SignInPage.");
        }
    }

private IEnumerator LoginUser(string userName, string password) {
    var jsonBody = JsonConvert.SerializeObject(new {
        action = "login",
        userName = userName,
        password = password
    });

    using (UnityWebRequest www = new UnityWebRequest("https://xiaosong.fr/decomaison/api/user_api.php/login", "POST")) {
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody));
        www.uploadHandler.contentType = "application/json";
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) {
            var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(www.downloadHandler.text);
            int userId = Convert.ToInt32(responseData["user_id"]);
            bool isAdmin = Convert.ToBoolean(responseData["is_admin"]);
            string email = responseData["email"].ToString();

            UserManager.Instance.LogIn(userId, isAdmin, userName, email);

            // 获取愿望清单
            yield return _monoBehaviour.StartCoroutine(UserManager.Instance.FetchWishList());

            ((PagesManager)_monoBehaviour).ShowPage("SettingsPage");
        } else {
            Debug.Log("Login failed: " + www.error);
        }
    }
}


    private IEnumerator RegisterUser(string userName, string password, string email) {
        var jsonBody = JsonConvert.SerializeObject(new {
            action = "register",
            userName = userName,
            password = password,
            email = email
        });

        using (UnityWebRequest www = new UnityWebRequest("https://xiaosong.fr/decomaison/api/user_api.php/register", "POST")) {
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody));
            www.uploadHandler.contentType = "application/json";
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) {
                Debug.Log("Register successful: " + www.downloadHandler.text);
            } else {
                Debug.Log("Register failed: " + www.error);
            }
        }
    }
}