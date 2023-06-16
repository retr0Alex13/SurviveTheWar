using UnityEngine;

namespace OM
{
    public class ItemSOHolder : MonoBehaviour
    {
        [SerializeField] private ItemSO itemSO;
        public ItemSO ItemSO { get { return itemSO; } }
        [field: SerializeField] public float CurrentDurability;

        public void DecreaseDurability(float amount)
        {
            CurrentDurability -= amount;
            if (CurrentDurability < 0)
                CurrentDurability = 0;
        }

    }
}