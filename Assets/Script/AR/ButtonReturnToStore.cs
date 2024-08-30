using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonReturnToStore : MonoBehaviour
{
    public Button buttonReturnToStore;
    public string StoreSceneName="StoreScene";

    void OnEnable()
    {
        buttonReturnToStore = GetComponent<Button>();
        buttonReturnToStore.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick ()
    {
        SceneManager.LoadScene(StoreSceneName, LoadSceneMode.Single);
    }
}
