using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemSO")]
public class ItemSO : ScriptableObject {

    public Item.ItemType itemType;
    public string itemName;
    public GameObject prefab;
}
