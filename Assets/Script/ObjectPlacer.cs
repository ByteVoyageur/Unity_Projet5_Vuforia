using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject SelectedObject { get; private set; }
    public bool IsPlaced { get; private set; }
    private bool isPlaced;
    private int frameCount = 0;
    private int hitErrorFrameCount = 0;
    private List<GameObject> placedObjects = new List<GameObject>();

    public bool IsObjectPlaced() { return isPlaced; }
    public bool HasSelectedObject() { return SelectedObject != null; }
    public void SetObjectPlaced(bool value) { isPlaced = value; IsPlaced = value; }

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
        if (SelectedObject != null)
        {
            if (!placedObjects.Contains(SelectedObject))
            {
                placedObjects.Add(SelectedObject);
            }
        }

        SelectedObject = obj;
        DisableColliders(SelectedObject);

        if (!SelectedObject.activeSelf)
        {
            SelectedObject.SetActive(true);
        }

        SetObjectPlaced(false);

        DoubleTapHandler doubleTapHandler = SelectedObject.GetComponent<DoubleTapHandler>();
        if (doubleTapHandler == null)
        {
            doubleTapHandler = SelectedObject.AddComponent<DoubleTapHandler>();
        }
        doubleTapHandler.infoPanelHandler = SelectedObject.GetComponentInChildren<InfoPanelHandler>();

        Debug.Log("SetSelectedObject called, object instantiated and activated.");
    }

    public void PlaceObject(SimpleHitTestResult result)
    {
        if (SelectedObject != null && !IsObjectPlaced())
        {
            SelectedObject.SetActive(true);
            SelectedObject.transform.position = result.Position;
            SetObjectPlaced(true);

            EnableColliders(SelectedObject);

            placedObjects.Add(SelectedObject);
            SelectedObject = null;

            Debug.Log($"Object placed at: {result.Position}, object is now active.");
        }
        else
        {
            Debug.LogWarning("No object selected to place or object is already placed.");
        }
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        if (!IsObjectPlaced() && SelectedObject != null)
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

        if (SelectedObject != null && !IsObjectPlaced())
        {
            Camera arCamera = Camera.main;
            if (arCamera != null)
            {
                Ray ray = new Ray(arCamera.transform.position, arCamera.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    SelectedObject.transform.position = hitInfo.point;

                    frameCount++;
                    if (frameCount >= 300)
                    {
                        frameCount = 0;
                        Debug.Log("Object position updated to: " + hitInfo.point);
                    }
                }
                else
                {
                    hitErrorFrameCount++;
                    if (frameCount >= 300)
                    {
                    Debug.LogWarning("Raycast did not hit any surface.");
                    }
                }
            }
        }
    }

    private void EnableGroundPlaneCollider()
    {
        GameObject groundPlaneStage = GameObject.Find("Ground Plane Stage");
        if (groundPlaneStage == null)
        {
            Debug.LogError("Ground Plane Stage not found.");
            return;
        }
        
        BoxCollider boxCollider = groundPlaneStage.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("Ground Plane Stage does not have a BoxCollider.");
            return;
        }

        if (!boxCollider.enabled)
        {
            boxCollider.enabled = true;
            Debug.Log("Enabled BoxCollider on Ground Plane Stage.");
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