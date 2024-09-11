using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public static class FooterController
{
    private static string arSceneName = "SampleScene";
    private static Label wishListCounter;

    // Initialize all buttons for Footer
    public static void InitializeFooter(VisualElement root, PagesManager pagesManager)
    {
        var homeFooter = root.Q<VisualElement>("HomeFooter");
            homeFooter.RegisterCallback<ClickEvent>(evt =>
            { 
                pagesManager.ShowPage("WelcomePage"); 
            });

        var settingFooter = root.Q<VisualElement>("SettingFooter");
            settingFooter.RegisterCallback<ClickEvent>(evt =>
            { 
                // Implement setting page navigation
                Debug.Log("Setting footer clicked."); 
            });

        var arModeFooter = root.Q<VisualElement>("ARModeFooter");
            arModeFooter.RegisterCallback<ClickEvent>(evt =>
            { 
                // Implement AR mode page navigation
                SceneManager.LoadScene(arSceneName, LoadSceneMode.Single);
                Debug.Log("AR Mode footer clicked."); 
            });

        var wishListFooter = root.Q<VisualElement>("WishListFooter");
            wishListFooter.RegisterCallback<ClickEvent>(evt =>
            {
                pagesManager.ShowPage("WishListPage");
            });

            // Initialize WishListCounter and subscribe to updates
            wishListCounter = wishListFooter.Q<Label>("WishListCounter");
            if (wishListCounter != null)
            {
                // Unsubscribe to prevent duplicate subscriptions
                WishListManager.Instance.OnItemAddedToWishList -= UpdateWishListCounter;
                WishListManager.Instance.OnItemRemovedFromWishList -= UpdateWishListCounter;
                
                // Subscribe to update events
                WishListManager.Instance.OnItemAddedToWishList += UpdateWishListCounter;
                WishListManager.Instance.OnItemRemovedFromWishList += UpdateWishListCounter;

                // Call the no-argument version to initialize
                UpdateWishListCounter();
            }
            else
            {
                Debug.LogError("WishListCounter element not found.");
            }
    }

    // Method to update WishList counter with parameter
    private static void UpdateWishListCounter(FurnitureSO itemData)
    {
        UpdateWishListCounter();
    }

    // Method to update WishList counter without parameter
    private static void UpdateWishListCounter()
    {
        if (wishListCounter != null)
        {
            int count = WishListManager.Instance.GetWishListItems().Count;
            wishListCounter.text = count.ToString();
            wishListCounter.style.display = count > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            Debug.Log($"Wish list updated. New count: {count}");
        }
    }
}