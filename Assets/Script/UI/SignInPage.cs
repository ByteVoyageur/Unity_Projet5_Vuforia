using UnityEngine;
using UnityEngine.UIElements;

public class SignInPage : Page
{
    public SignInPage(VisualTreeAsset visualTreeAsset) : base(visualTreeAsset)
    {
    }

    public static SignInPage CreateInstance(VisualTreeAsset visualTreeAsset)
    {
        return new SignInPage(visualTreeAsset);
    }

    public void Initialize(PagesManager pagesManager)
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
    }
}