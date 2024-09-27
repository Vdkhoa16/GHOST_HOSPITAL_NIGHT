using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public GameObject prefab;

    public bool isKey; // kiểm tra item có phải là key hay k
    public int keyID; 

}
