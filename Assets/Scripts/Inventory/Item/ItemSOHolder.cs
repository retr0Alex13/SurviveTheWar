using UnityEngine;

namespace OM
{
    public class ItemSOHolder : MonoBehaviour
    {
        [SerializeField] private ItemSO itemSO;
        [field:SerializeField] public float ItemCapacity { get; set; }
        public ItemSO ItemSO { get { return itemSO; } }
    }
}