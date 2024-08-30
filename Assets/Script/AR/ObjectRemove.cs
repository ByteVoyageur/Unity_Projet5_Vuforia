using UnityEngine;
using UnityEngine.UI;

public class ObjectRemove : MonoBehaviour
{
    private Button deleteButton;
    private ObjectPlacer objectPlacer;

    private void Start()
    {
        // 获取 Panel
        Transform panelTransform = transform.Find("Canvas/Panel");
        if (panelTransform == null)
        {
            Debug.LogError("Panel not found. Make sure it is a child of Canvas.");
            return;
        }
        else
        {
            Debug.Log("Panel found");
        }

        // 找到 DeleteButton
        Transform deleteButtonTransform = panelTransform.Find("DeleteButton");
        if (deleteButtonTransform == null)
        {
            Debug.LogError("DeleteButton not found. Ensure DeleteButton is a child of Panel.");
            return;
        }
        else
        {
            Debug.Log("DeleteButton found");
        }

        deleteButton = deleteButtonTransform.GetComponent<Button>();
        if (deleteButton == null)
        {
            Debug.LogError("DeleteButton component is null. Ensure the object has a Button component.");
            return;
        }
        else
        {
            Debug.Log("DeleteButton component found");
        }

        // 为 DeleteButton 设置事件监听器
        deleteButton.onClick.AddListener(DeleteObject);
        Debug.Log("Listener added to DeleteButton");

        // 获取 ObjectPlacer 脚本
        objectPlacer = FindObjectOfType<ObjectPlacer>();
        if (objectPlacer == null)
        {
            Debug.LogError("ObjectPlacer component not found in the scene.");
        }
        else
        {
            Debug.Log("ObjectPlacer found");
        }
    }

    private void DeleteObject()
    {
        Debug.Log("Delete button clicked");

        // Deactivate the object
        gameObject.SetActive(false);

        Destroy(gameObject);

        // Inform ObjectPlacer to remove this object from the placed objects list
        if (objectPlacer != null)
        {
            objectPlacer.RemovePlacedObject(gameObject);
        }
        else
        {
            Debug.LogWarning("ObjectPlacer is null, cannot remove placed object.");
        }

        Debug.Log("Object deleted and removed from placed objects list");
    }
}