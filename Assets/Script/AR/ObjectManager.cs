using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour
{
    public ObjectPlacer objectPlacer;
    public Button buttonTemplate;
    public Transform buttonContainer;

    private List<GameObject> objectPrefabs = new List<GameObject>();
    private List<Button> buttons = new List<Button>();

    void Start()
    {
        foreach (var prefab in objectPrefabs)
        {
            prefab.SetActive(false);
        }
    }

    public void AddObject(GameObject prefab, Texture2D icon)
{
    if (prefab == null)
    {
        Debug.LogError("AddObject received a null prefab");
        return;
    }

    Debug.Log($"AddObject called with prefab: {prefab.name}");
    
    objectPrefabs.Add(prefab);

    var button = Instantiate(buttonTemplate, buttonContainer);
    if (button == null)
    {
        Debug.LogError("Failed to create button from template");
        return;
    }

    button.GetComponent<Image>().sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), new Vector2(0.5f, 0.5f));
    button.onClick.AddListener(() => SelectObject(prefab));
    buttons.Add(button);

    Debug.Log($"Successfully added object {prefab.name} to ObjectManager");
}
    private void SelectObject(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("SelectObject received a null prefab");
            return;
        }

        if (objectPlacer.SelectedObject != null)
        {
            Destroy(objectPlacer.SelectedObject);
        }

        GameObject newObject = Instantiate(prefab);
        if (newObject == null)
        {
            Debug.LogError("Failed to instantiate newObject from prefab");
            return;
        }

        objectPlacer.SetSelectedObject(newObject);
        Debug.Log($"Object {newObject.name} selected and instantiated.");
    }

    private void DisableAllObjects()
    {
        foreach (var obj in objectPrefabs)
        {
            obj.SetActive(false);
        }
    }
}