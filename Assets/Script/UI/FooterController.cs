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
        if (homeFooter != null)
        {
            homeFooter.RegisterCallback<ClickEvent>(evt =>
            { 
                pagesManager.ShowPage("HomePage"); 
            });
        }
        else
        {
            Debug.LogError("HomeFooter element not found.");
        }

        var settingFooter = root.Q<VisualElement>("SettingFooter");
        if (settingFooter != null)
        {
            settingFooter.RegisterCallback<ClickEvent>(evt =>
            { 
                // Implement setting page navigation
                Debug.Log("Setting footer clicked."); 
            });
        }
        else
        {
            Debug.LogError("SettingFooter element not found.");
        }

        var arModeFooter = root.Q<VisualElement>("ARModeFooter");
        if (arModeFooter != null)
        {
            arModeFooter.RegisterCallback<ClickEvent>(evt =>
            { 
                // Implement AR mode page navigation
                SceneManager.LoadScene(arSceneName, LoadSceneMode.Single);
                Debug.Log("AR Mode footer clicked."); 
            });
        }
        else
        {
            Debug.LogError("ARModeFooter element not found.");
        }

        var wishListFooter = root.Q<VisualElement>("WishListFooter");
        if (wishListFooter != null)
        {
            wishListFooter.RegisterCallback<ClickEvent>(evt =>
            {
                pagesManager.ShowPage("WishListPage");
            });

            // Initialize WishListCounter and subscribe to updates
            wishListCounter = wishListFooter.Q<Label>("WishListCounter");
            if (wishListCounter != null)
            {
                WishListManager.Instance.OnItemAddedToWishList -= UpdateWishListCounter;
                WishListManager.Instance.OnItemRemovedFromWishList -= UpdateWishListCounter;
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
        else
        {
            Debug.LogError("WishListFooter element not found.");
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