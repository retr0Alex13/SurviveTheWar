using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemSO")]
public class ItemSO : ScriptableObject
{

    [SerializeField] private Item.ItemType itemType;
    [SerializeField] private string itemName;
    [SerializeField] private GameObject prefab;

    public GameObject Prefab { get { return prefab; } }
}
