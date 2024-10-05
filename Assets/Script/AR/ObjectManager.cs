using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour {
    public ObjectPlacer objectPlacer;
    public GameObject buttonTemplate;  // Use GameObject for Prefab
    public Transform buttonContainer;
    private List<GameObject> objectPrefabs = new List<GameObject>();
    private List<Button> buttons = new List<Button>();

    void Start() {
        if (buttonContainer == null) {
            buttonContainer = GameObject.Find("Canvas_list_items/ContainerList")?.transform;
            if (buttonContainer == null) {
                Debug.LogError("Button container not assigned and couldn't be found in the scene.");
                return;
            }
        }
        RefreshButtons();
    }

    public void RefreshButtons() {
        if (buttonTemplate == null) {
            Debug.LogError("Button template is not assigned.");
            return;
        }

        foreach (var button in buttons) {
            Destroy(button.gameObject);
        }
        buttons.Clear();
        objectPrefabs.Clear();

        int count = 0;
        foreach (var item in WishListManager.Instance.GetWishListItems()) {
            AddObject(item.prefab, item.icon, count);
            count++;
        }
    }

    public void AddObject(GameObject prefab, Texture2D icon, int count) {
        if (prefab == null) {
            Debug.LogError("AddObject received a null prefab");
            return;
        }
        if (buttonTemplate == null) {
            Debug.LogError("Button template is not assigned.");
            return;
        }
        if (buttonContainer == null) {
            Debug.LogError("Button container is not assigned.");
            return;
        }

        Debug.Log($"AddObject called with prefab: {prefab.name}");
        objectPrefabs.Add(prefab);

        var buttonObject = Instantiate(buttonTemplate, buttonContainer);
        buttonObject.SetActive(true);

        var button = buttonObject.GetComponent<Button>();
        var image = buttonObject.GetComponent<Image>();

        if (image != null) {
            image.sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), new Vector2(0.5f, 0.5f));
        }

        button.onClick.AddListener(() => SelectObject(prefab));
        buttons.Add(button);

        RectTransform rectTransform = button.GetComponent<RectTransform>();
        Vector2 initialPosition = new Vector2(-462, -174);
        float buttonSpacing = 10f;
        float xPos = initialPosition.x + (rectTransform.sizeDelta.x * count + buttonSpacing * count);
        rectTransform.anchoredPosition = new Vector2(xPos, initialPosition.y);

        Debug.Log($"Successfully added object {prefab.name} to ObjectManager");
    }

    private void SelectObject(GameObject prefab) {
        if (prefab == null) {
            Debug.LogError("SelectObject received a null prefab");
            return;
        }
        if (objectPlacer.SelectedObject != null) {
            Destroy(objectPlacer.SelectedObject);
        }

        GameObject newObject = Instantiate(prefab);
        if (newObject == null) {
            Debug.LogError("Failed to instantiate newObject from prefab");
            return;
        }

        var infoPanelHandler = newObject.GetComponentInChildren<InfoPanelHandler>();
        var clickableObject = newObject.GetComponentInChildren<ClickableObject>();

        if (clickableObject != null && infoPanelHandler != null) {
            clickableObject.infoPanelHandler = infoPanelHandler;
        }

        var item = GetItemByPrefab(prefab);
        if (item != null) {
            if (infoPanelHandler != null) {
                infoPanelHandler.itemData = item;
                Debug.Log($"Assigned itemData to InfoPanelHandler: Name={item.name}, Description={item.description}, Price={item.price}");
            }
        }

        objectPlacer.SetSelectedObject(newObject);
        Debug.Log($"Object {newObject.name} selected and instantiated.");
    }

    private WishListManager.Item GetItemByPrefab(GameObject prefab) {
        string prefabName = prefab.name.Replace("(Clone)", "").Trim();
        Debug.Log($"Looking for item with prefabName: {prefabName}");

        var wishListItems = WishListManager.Instance.GetWishListItems();
        foreach (var item in wishListItems) {
            Debug.Log($"Comparing with item.prefabName: {item.prefabName}");
            if (item.prefabName == prefabName) {
                Debug.Log("Match found");
                return item;
            }
        }
        Debug.LogWarning("No matching item found for prefab");
        return null;
    }
}