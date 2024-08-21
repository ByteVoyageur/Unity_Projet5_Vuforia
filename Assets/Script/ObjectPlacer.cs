using UnityEngine;
using Lean.Touch;
using Vuforia;

public class ObjectPlacer : MonoBehaviour
{
    private GameObject selectedObject;
    private bool isPlaced;
    private int frameCount = 0; 

    void Start()
    {
        isPlaced = false;
    }

    public void SetSelectedObject(GameObject obj)
    {
        if (selectedObject != null)
        {
            Destroy(selectedObject);
        }

        selectedObject = Instantiate(obj);
        selectedObject.SetActive(true); 
        isPlaced = false;
        Debug.Log("SetSelectedObject called, object instantiated and activated.");
    }

    public bool HasSelectedObject()
    {
        return selectedObject != null;
    }

    public bool IsObjectPlaced()
    {
        return isPlaced;
    }

    public void PlaceObject(HitTestResult result)
    {
        if (selectedObject != null && !isPlaced)
        {
            selectedObject.SetActive(true); 
            selectedObject.transform.position = result.Position;
            isPlaced = true;
            Debug.Log($"Object placed at: {result.Position}, object is now active.");
        }
        else
        {
            Debug.LogWarning("No object selected to place or object is already placed.");
        }
    }

    void Update()
    {
        if (selectedObject != null && !isPlaced)
        {
            Camera arCamera = Camera.main;

            if (arCamera != null)
            {
                Ray ray = new Ray(arCamera.transform.position, arCamera.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    selectedObject.transform.position = hitInfo.point;

                    frameCount++;
                    if (frameCount >= 120)
                    {
                        frameCount = 0; 
                        Debug.Log("Object position updated to: " + hitInfo.point);
                    }
                }
                else
                {
                    Debug.LogWarning("Raycast did not hit any surface.");
                }
            }
        }
    }
}