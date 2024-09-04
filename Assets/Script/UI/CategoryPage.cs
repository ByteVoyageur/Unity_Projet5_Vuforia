using UnityEngine;
using UnityEngine.UIElements;

public class CategoryPage : Page
{
    public CategoryPage(VisualTreeAsset visualTreeAsset) : base(visualTreeAsset)
    {
    }

    public static CategoryPage CreateInstance(VisualTreeAsset visualTreeAsset)
    {
        var instance = new CategoryPage(visualTreeAsset);
        instance.Initialize();
        return instance;
    }

    private void Initialize()
    {
        Root.Q<Label>("CategoryTitle").text = "Category Page";
        // Temporarily skip item population logic
        Debug.Log("Initialized basic CategoryPage.");
    }
}