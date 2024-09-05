using UnityEngine;

[CreateAssetMenu(menuName = "ARApp/FurnitureSO")]
public class FurnitureSO : ScriptableObject
{
    public string itemName;
    public string description;
    public GameObject furniturePrefab;
    public float price;
    public Vector3 size;
    public string imageName; 
}