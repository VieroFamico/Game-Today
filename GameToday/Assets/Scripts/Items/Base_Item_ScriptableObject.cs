using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemSO", order = 1)]
public class Base_Item_ScriptableObject : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite ItemSprite;

}
