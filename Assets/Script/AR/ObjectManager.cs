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

        // Instantiate the button from the prefab (template)
        var buttonObject = Instantiate(buttonTemplate, buttonContainer);
        buttonObject.SetActive(true); // Make sure the new button is active

        var button = buttonObject.GetComponent<Button>();
        var image = buttonObject.GetComponent<Image>();

        if (image != null) {
            image.sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), new Vector2(0.5f, 0.5f));
        }

        button.onClick.AddListener(() => SelectObject(prefab));
        buttons.Add(button);

        // Set position for the button
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        float buttonSpacing = 100f; // Adjust this value based on the desired spacing
        float xPos = rectTransform.sizeDelta.x * count + buttonSpacing * count;
        rectTransform.anchoredPosition = new Vector2(xPos, 0);

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

        objectPlacer.SetSelectedObject(newObject);
        Debug.Log($"Object {newObject.name} selected and instantiated.");
    }

    private void DisableAllObjects() {
        foreach (var obj in objectPrefabs) {
            obj.SetActive(false);
        }
    }
}