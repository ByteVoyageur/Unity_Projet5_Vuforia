using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ViewInARButtonController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string arSceneName = "SampleScene"; 

    private void OnEnable()
    {
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument is not assigned.");
            return;
        }

        var root = uiDocument.rootVisualElement;
        var viewInARButton = root.Q<Button>("ViewInARButton");
        if (viewInARButton == null)
        {
            Debug.LogError("ViewInARButton is not found in the UXML.");
            return;
        }

        viewInARButton.clicked += OnViewInARButtonClicked;
    }

    private void OnDisable()
    {
        if (uiDocument == null)
        {
            return;
        }

        var root = uiDocument.rootVisualElement;
        var viewInARButton = root.Q<Button>("ViewInARButton");

        if (viewInARButton != null)
        {
            viewInARButton.clicked -= OnViewInARButtonClicked;
        }
    }

    private void OnViewInARButtonClicked()
    {
        SceneManager.LoadScene(arSceneName, LoadSceneMode.Single);
    }
}