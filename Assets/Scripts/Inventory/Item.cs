using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ItemType
{
    isNone,
    isKey,
    isLetter,
    isFlashLight
}

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]

public class Item : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public GameObject prefab;

    //public bool isKey; // kiểm tra item có phải là key hay k

    public ItemType itemType;
    public int keyID; 

}
