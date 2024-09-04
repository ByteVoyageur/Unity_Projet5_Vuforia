using UnityEngine;
using UnityEngine.UIElements;

public class WelcomePage : Page
{
    public WelcomePage(VisualTreeAsset visualTreeAsset) : base(visualTreeAsset)
    {
    }

    public static WelcomePage CreateInstance(VisualTreeAsset visualTreeAsset)
    {
        return new WelcomePage(visualTreeAsset);
    }

    public void Initialize(PagesManager pagesManager)
    {
        // Find HomeFooter element and add click event to navigate back to HomePage
        var homeFooter = Root.Q<VisualElement>("HomeFooter");
        if (homeFooter != null)
        {
            homeFooter.RegisterCallback<ClickEvent>(evt =>
            {
                pagesManager.ShowPage("HomePage");
            });
        }
        else
        {
            Debug.LogError("HomeFooter element not found on WelcomePage.");
        }

        // Find ARModeFooter element and add click event to navigate to CategoryPage
        var arModeFooter = Root.Q<VisualElement>("ARModeFooter");
        if (arModeFooter != null)
        {
            arModeFooter.RegisterCallback<ClickEvent>(evt =>
            {
                pagesManager.ShowPage("CategoryPage");
            });
        }
        else
        {
            Debug.LogError("ARModeFooter element not found on WelcomePage.");
        }
    }
}