using UnityEngine;
using Lean.Touch;

public class DoubleTapHandler : MonoBehaviour
{
    public InfoPanelHandler infoPanelHandler;
    private ObjectPlacer objectPlacer;
    private LeanFingerTap leanFingerTap;

    void Start()
    {
        objectPlacer = FindObjectOfType<ObjectPlacer>();
        leanFingerTap = GetComponent<LeanFingerTap>();

        if (leanFingerTap == null)
        {
            leanFingerTap = gameObject.AddComponent<LeanFingerTap>();
        }

        leanFingerTap.RequiredTapCount = 2;
        leanFingerTap.OnFinger.AddListener(OnDoubleTap);
    }

    private void OnDoubleTap(LeanFinger finger)
    {
        if (objectPlacer != null)
        {
            CancelPlacement();
        }
    }

    private void CancelPlacement()
    {
        objectPlacer.SetSelectedObject(transform.gameObject);
        objectPlacer.RemovePlacedObject(gameObject);
        objectPlacer.SetObjectPlaced(false); 
    }
}