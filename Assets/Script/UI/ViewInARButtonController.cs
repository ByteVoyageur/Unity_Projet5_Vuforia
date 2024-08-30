using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ViewInARButtonController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string arSceneName = "SampleScene";

    private Button viewInARButton;

    void OnEnable()
    {
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument is not assigned.");
            return;
        }

        var root = uiDocument.rootVisualElement;
        viewInARButton = root.Q<Button>("ViewInARButton");

        if (viewInARButton == null)
        {
            Debug.LogError("ViewInARButton is not found in the UXML.");
            return;
        }

        viewInARButton.clicked += OnButtonClick;
    }

    void OnDisable()
    {
        if (viewInARButton != null)
        {
            viewInARButton.clicked -= OnButtonClick;
        }
    }

    void OnButtonClick()
    {
        SceneManager.LoadScene(arSceneName, LoadSceneMode.Single);
    }
}