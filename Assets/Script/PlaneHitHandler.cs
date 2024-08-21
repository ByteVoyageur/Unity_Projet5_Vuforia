// PlaneHitHandler.cs
using UnityEngine;
using Vuforia;

public class PlaneHitHandler : MonoBehaviour
{
    public ObjectPlacer objectPlacer;

    public void OnInteractiveHitTest(HitTestResult result)
    {
        if (objectPlacer != null && objectPlacer.HasSelectedObject() && !objectPlacer.IsObjectPlaced())
        {
            SimpleHitTestResult simpleResult = new SimpleHitTestResult
            {
                Position = result.Position
            };
            objectPlacer.PlaceObject(simpleResult); // 使用自定义的 SimpleHitTestResult
            Debug.Log("Plane hit detected, executing place object.");
        }
    }
}