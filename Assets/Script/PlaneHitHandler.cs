using UnityEngine;
using Vuforia;

public class PlaneHitHandler : MonoBehaviour
{
    public ObjectPlacer objectPlacer;

    public void OnInteractiveHitTest(HitTestResult result)
    {
        if (objectPlacer != null && objectPlacer.HasSelectedObject() && !objectPlacer.IsObjectPlaced())
        {
            objectPlacer.PlaceObject(result);
            Debug.Log("Plane hit detected, executing place object.");
        }
    }
}