using UnityEngine;

[CreateAssetMenu(menuName = "ARApp/FurnitureSO")]
public class FurnitureSO : ScriptableObject
{
    public string itemName;
    public string description;
    public float price;
    public Texture2D icon;  
    public GameObject prefab;
    public string imageUrl;
}