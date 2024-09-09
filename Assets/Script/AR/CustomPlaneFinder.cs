using UnityEngine;
using Vuforia;
using Lean.Touch;

public class CustomPlaneFinder : MonoBehaviour
{
    public PlaneFinderBehaviour planeFinder;
    public ContentPositioningBehaviour contentPositioning;
    public GameObject furniturePrefab;

    private bool isSurfacePlaced = false;

    private void OnEnable()
    {
        LeanTouch.OnFingerTap += HandleFingerTap;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerTap -= HandleFingerTap;
    }

    private void HandleFingerTap(Lean.Touch.LeanFinger finger)
    {
        if (isSurfacePlaced)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("DetectedPlane"))
            {
                PlaceFurniture(hit.point);
            }
        }
    }

    private void PlaceFurniture(Vector3 position)
    {
        if (furniturePrefab != null)
        {
            var newFurniture = Instantiate(furniturePrefab, position, Quaternion.identity);
            newFurniture.transform.parent = planeFinder.transform;

            // Set isSurfacePlaced to true to prevent further placement
            isSurfacePlaced = true;
        }
    }

    public void ResetPlacement()
    {
        foreach (Transform child in planeFinder.transform)
        {
            if (child.CompareTag("Furniture"))
            {
                Destroy(child.gameObject);
            }
        }

        isSurfacePlaced = false;
    }
}