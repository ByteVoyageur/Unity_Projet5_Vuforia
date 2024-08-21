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
        GameObject newObject = Instantiate(prefab, Camera.main.transform.position + Camera.main.transform.forward * 2, Quaternion.identity);
        SetObjectTransparency(newObject, 0.5f);
        objectPlacer.SetSelectedObject(newObject); 
    }

    private void SetObjectTransparency(GameObject obj, float alpha)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material;
            Color color = mat.color;
            color.a = alpha;
            mat.color = color;
            mat.SetFloat("_Mode", 3); 
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }
    }
}