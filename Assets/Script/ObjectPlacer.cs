using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class ObjectPlacer : MonoBehaviour
{
    private GameObject selectedObject;
    private bool isPlaced;
    private int frameCount = 0;
    private List<GameObject> placedObjects = new List<GameObject>();

    void Start()
    {
        isPlaced = false;

        EnableGroundPlaneCollider();
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += HandleFingerDown;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= HandleFingerDown;
    }

    public void SetSelectedObject(GameObject obj)
    {
        if (selectedObject != null)
        {
            placedObjects.Add(selectedObject);
            selectedObject = null;
        }

        selectedObject = Instantiate(obj);
        selectedObject.SetActive(true);
        isPlaced = false;

        ClickableObject clickable = selectedObject.AddComponent<ClickableObject>();
        clickable.infoPanelHandler = selectedObject.GetComponentInChildren<InfoPanelHandler>();

        DisableColliders(selectedObject);

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

    public void PlaceObject(SimpleHitTestResult result)
    {
        if (selectedObject != null && !isPlaced)
        {
            selectedObject.SetActive(true);
            selectedObject.transform.position = result.Position;
            isPlaced = true;

            EnableColliders(selectedObject);

            placedObjects.Add(selectedObject);
            selectedObject = null;

            Debug.Log($"Object placed at: {result.Position}, object is now active.");
        }
        else
        {
            Debug.LogWarning("No object selected to place or object is already placed.");
        }
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        if (!isPlaced && selectedObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                SimpleHitTestResult hitTestResult = new SimpleHitTestResult
                {
                    Position = hitInfo.point
                };
                PlaceObject(hitTestResult);
            }
        }
    }

    void Update()
    {
        EnableGroundPlaneCollider();

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
                    if (frameCount >= 600)
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

    private void EnableGroundPlaneCollider()
    {
        BoxCollider boxCollider = GameObject.Find("Ground Plane Stage").GetComponent<BoxCollider>();
        if (boxCollider != null && !boxCollider.enabled)
        {
            boxCollider.enabled = true;
        }
    }

    private void EnableColliders(GameObject obj)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
    }

    private void DisableColliders(GameObject obj)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }

        public void RemovePlacedObject(GameObject obj)
    {
        if (placedObjects.Contains(obj))
        {
            placedObjects.Remove(obj);
            Debug.Log("Object removed from placed objects list.");
        }
    }
}