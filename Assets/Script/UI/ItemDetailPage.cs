using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ItemDetailPage : Page
{
    private string arSceneName = "SampleScene"; // Specify your AR scene name here

    public ItemDetailPage(VisualTreeAsset visualTreeAsset) : base(visualTreeAsset)
    {
    }

    public static ItemDetailPage CreateInstance(VisualTreeAsset visualTreeAsset)
    {
        return new ItemDetailPage(visualTreeAsset);
    }

    public void Initialize()
    {
        var viewInARButton = Root.Q<Button>("ViewInARButton");

        if (viewInARButton != null)
        {
            viewInARButton.clicked += OnViewInARButtonClick;
        }
        else
        {
            Debug.LogError("ViewInARButton element not found on ItemDetailPage.");
        }
    }

    private void OnViewInARButtonClick()
    {
        SceneManager.LoadScene(arSceneName, LoadSceneMode.Single);
    }
}