using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemSO")]
    public class ItemSO : ScriptableObject
    {

        public enum ItemType
        {
            None,
            CraftingMaterial,
            Usable
        }


        [SerializeField] private ItemType itemType;
        [SerializeField] public string itemName;
        [SerializeField] public int quantity;
        [SerializeField] private GameObject prefab;
        [SerializeField] public Sprite icon;

        public GameObject Prefab { get { return prefab; } }
    }
}
