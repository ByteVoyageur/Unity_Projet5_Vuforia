using UnityEngine.UIElements;

public class HomePage : Page
{
    public HomePage(VisualTreeAsset visualTreeAsset) : base(visualTreeAsset) { }

    public static HomePage CreateInstance(VisualTreeAsset visualTreeAsset)
    {
        return new HomePage(visualTreeAsset);
    }
}