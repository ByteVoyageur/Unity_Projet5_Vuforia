// using UnityEngine;
// using UnityEngine.TestTools;
// using NUnit.Framework;
// using System.Collections;
// using UnityEngine.UIElements;

// public class SignInPageTests
// {
//     private GameObject _testGameObject;
//     private MonoBehaviour _monoBehaviour;
//     private SignInPage _signInPage;

//     [SetUp]
//     public void Setup()
//     {
//         _testGameObject = new GameObject();
//         _monoBehaviour = _testGameObject.AddComponent<MonoBehaviour>();

//         VisualTreeAsset visualTreeAsset = ScriptableObject.CreateInstance<VisualTreeAsset>();

//         _signInPage = new SignInPage(visualTreeAsset, _monoBehaviour);

//         _signInPage.Root = new VisualElement();

//         var emailField = new TextField("EmailOrUserName");
//         emailField.value = "testUser";
//         _signInPage.Root.Add(emailField);

//         var passwordField = new TextField("PassWord");
//         passwordField.value = "testPassword";
//         _signInPage.Root.Add(passwordField);

//         var loginButton = new Button();
//         loginButton.name = "LoginButton";
//         _signInPage.Root.Add(loginButton);
//     }

//     [UnityTest]
//     public IEnumerator LoginUserTest()
//     {
//         IEnumerator loginCoroutine = _signInPage.LoginUser("testUser", "testPassword");

//         Assert.IsTrue(loginCoroutine.MoveNext(), "LoginUser coroutine did not start correctly.");


//         yield return null;
//     }

//     [TearDown]
//     public void Teardown()
//     {
//         Object.DestroyImmediate(_testGameObject);
//     }
// }
