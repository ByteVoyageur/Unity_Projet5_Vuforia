using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonReturnToStore : MonoBehaviour
{
    public Button buttonReturnToStore;
    public string storeSceneName = "StoreScene";
    private bool isReturningToWishListPage = true; // Flag to indicate returning page

    void OnEnable()
    {
        buttonReturnToStore = GetComponent<Button>();
        buttonReturnToStore.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick ()
    {
        // Load store scene
        SceneManager.sceneLoaded += OnStoreSceneLoaded;
        SceneManager.LoadScene(storeSceneName, LoadSceneMode.Single);
    }

    private void OnStoreSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isReturningToWishListPage)
        {
            PagesManager pagesManager = FindObjectOfType<PagesManager>();
            if (pagesManager != null)
            {
                pagesManager.ShowPage("WishListPage");
                Debug.Log("Returned to WishListPage");
            }
            else
            {
                Debug.LogError("PagesManager not found in StoreScene.");
            }

            // Reset the flag and unsubscribe from the event
            isReturningToWishListPage = false;
            SceneManager.sceneLoaded -= OnStoreSceneLoaded;
        }
    }
}