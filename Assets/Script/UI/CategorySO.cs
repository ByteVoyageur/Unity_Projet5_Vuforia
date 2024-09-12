using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ARApp/CategorySO")]
public class CategorySO : ScriptableObject
{
    public FurnitureSO[] category;
    public Texture2D categoryImage; 
}
