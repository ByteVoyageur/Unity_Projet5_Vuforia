using UnityEngine;
using UnityEngine.UI;

public class ObjectRemove : MonoBehaviour
{
    private Button deleteButton;
    private ObjectPlacer objectPlacer;

    private void Start()
    {
        Transform panelTransform = transform.Find("Canvas/Panel");
        Transform deleteButtonTransform = panelTransform.Find("DeleteButton");

        deleteButton = deleteButtonTransform.GetComponent<Button>();

        deleteButton.onClick.AddListener(DeleteObject);

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