using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public GameObject lampPrefab;
    public GameObject cupboardPrefab;
    public ObjectPlacer objectPlacer;

    void Start()
    {
        Button lampButton = GameObject.Find("LampButton").GetComponent<Button>();
        Button cupboardButton = GameObject.Find("CupboardButton").GetComponent<Button>();

        lampButton.onClick.AddListener(() => SelectObject(lampPrefab));
        cupboardButton.onClick.AddListener(() => SelectObject(cupboardPrefab));

        DisableObjectsInitially(); 
    }

    private void DisableObjectsInitially()
    {
        lampPrefab.SetActive(false);
        cupboardPrefab.SetActive(false);
    }

    private void SelectObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab);
        objectPlacer.SetSelectedObject(newObject);
        Debug.Log("Object selected and instantiated.");
    }
}