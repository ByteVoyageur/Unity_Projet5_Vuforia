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
        return instance;
    }

    public void Initialize(PagesManager pagesManager)
    {
        Root.Q<Label>("CategoryTitle").text = "Category Page";

        FooterController.InitializeFooter(Root, pagesManager); 

        Debug.Log("Initialized basic CategoryPage.");
    }
}