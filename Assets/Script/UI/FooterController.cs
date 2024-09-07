using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class FooterController
{
    // Initialize all buttons fo Footer
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
        }
        else
        {
            Debug.LogError("WishListFooter element not found.");
        }
    }
}