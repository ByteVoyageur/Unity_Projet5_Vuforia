using System.Collections;
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
        WishListManager.Instance.PrintWishList();
        StartCoroutine(LoadARSceneWithVerification());
    }

    private IEnumerator LoadARSceneWithVerification()
    {
        SceneManager.LoadScene(arSceneName, LoadSceneMode.Additive);

        yield return new WaitForSeconds(1);

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            Debug.Log($"Loaded Scene: {scene.name}");
        }

        LoadWishListObjectsInAR();
    }

    private void LoadWishListObjectsInAR()
    {
        foreach (var item in WishListManager.Instance.GetWishListItems())
        {
            if (item != null && item.prefab != null)
            {
                Debug.Log($"Attempting to instantiate {item.itemName} in AR scene");
                Instantiate(item.prefab, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                Debug.LogError($"Item or its prefab is null: {item?.itemName}");
            }
        }
    }
}