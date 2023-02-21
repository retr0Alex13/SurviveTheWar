using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemSO")]
    public class ItemSO : ScriptableObject
    {

        [SerializeField] private Item.ItemType itemType;
        [SerializeField] public string itemName;
        [SerializeField] private int amount = 1;
        [SerializeField] private GameObject prefab;
        [SerializeField] public Sprite icon;

        public GameObject Prefab { get { return prefab; } }
    }
}
