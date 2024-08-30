using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public GameObject lampPrefab;
    public GameObject cupboardPrefab;
    public GameObject tablePrefab;
    public ObjectPlacer objectPlacer;

    void Start()
    {
        Button lampButton = GameObject.Find("LampButton").GetComponent<Button>();
        Button cupboardButton = GameObject.Find("CupboardButton").GetComponent<Button>();
        Button tableButton = GameObject.Find("TableButton").GetComponent<Button>();

        lampButton.onClick.AddListener(() => 
        {
            Debug.Log("Lamp button clicked"); 
            SelectObject(lampPrefab); 
        });

        cupboardButton.onClick.AddListener(() => 
        {
            Debug.Log("Cupboard button clicked"); 
            SelectObject(cupboardPrefab); 
        });

        tableButton.onClick.AddListener(() => 
        {
            Debug.Log("Table button clicked"); 
            SelectObject(tablePrefab); 
        });

        DisableObjectsInitially(); 
    }

    private void DisableObjectsInitially()
    {
        lampPrefab.SetActive(false);
        cupboardPrefab.SetActive(false);
        tablePrefab.SetActive(false);
    }

    private void SelectObject(GameObject prefab)
    {
        if (objectPlacer.SelectedObject != null)
        {
            Destroy(objectPlacer.SelectedObject);
        }

        GameObject newObject = Instantiate(prefab);
        objectPlacer.SetSelectedObject(newObject);
        Debug.Log("Object selected and instantiated.");
    }
}