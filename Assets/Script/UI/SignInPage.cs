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
}