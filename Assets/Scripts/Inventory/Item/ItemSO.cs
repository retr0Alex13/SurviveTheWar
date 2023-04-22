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
            Equipable,
            Eatable
        }


        [SerializeField] public ItemType itemType;
        [SerializeField] public string itemName;
        [SerializeField] private GameObject prefab;
        [SerializeField] public Sprite image;

        public GameObject Prefab { get { return prefab; } }
    }
}
