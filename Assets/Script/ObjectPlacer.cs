using UnityEngine;
using Lean.Touch;

public class ObjectPlacer : MonoBehaviour
{
    private GameObject selectedObject;
    private Vector3 initialScale;

    public Color unplacedColor = Color.green; 

    void Start()
    {
        LeanTouch.OnFingerTap += HandleFingerTap;
    }

    void OnDestroy()
    {
        LeanTouch.OnFingerTap -= HandleFingerTap;
    }

    public void SetSelectedObject(GameObject obj)
    {
        if (selectedObject != null)
        {
            Destroy(selectedObject);
        }

        selectedObject = obj;
        initialScale = selectedObject.transform.localScale;
        SetObjectColor(selectedObject, unplacedColor);
        selectedObject.SetActive(true);
    }

    private void HandleFingerTap(LeanFinger finger)
    {
        if (selectedObject != null && selectedObject.GetComponent<Renderer>().enabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                selectedObject.transform.position = hit.point;
                SetObjectColor(selectedObject, Color.white); 
                selectedObject = null; 
            }
        }
    }

    private void Update()
    {
        if (selectedObject != null)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                selectedObject.transform.position = hitInfo.point;
                selectedObject.transform.localScale = initialScale; 
            }
        }
    }

    private void SetObjectColor(GameObject obj, Color color)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material;
            mat.color = color;
        }
    }
}