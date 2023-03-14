using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] public List<ItemSO> items = new List<ItemSO>();

        public delegate void InventoryAction();
        public static event InventoryAction OnItemChanged;

        private void Start()
        {

        }

        public void AddItem(ItemSO item)
        {
            items.Add(item);
            OnItemChanged?.Invoke();
        }

        public void RemoveItem(ItemSO item)
        {
            items.Remove(item);
            OnItemChanged?.Invoke();
        }
    }

}
