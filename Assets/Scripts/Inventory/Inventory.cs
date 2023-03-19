using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] public List<ItemSO> items = new List<ItemSO>();
        [SerializeField] private int inventorySize = 4;
        [SerializeField] private InventoryMediator mediator;
        private bool isInventoryChanged;


        private void Start()
        {
            UpdateInventoryUI();
        }

        public void AddItem(ItemSO item)
        {
            items.Add(item);
            UpdateInventoryUI();
        }

        public void RemoveItem(ItemSO item)
        {
            items.Remove(item);
            UpdateInventoryUI();
        }

        private void UpdateInventoryUI()
        {
            isInventoryChanged = true;
            mediator.FireEvent(isInventoryChanged);
            isInventoryChanged = false;
        }

        public bool InventorySlotsAvailable()
        {
            if (items.Count >= inventorySize)
                return false;

            return true;
        }
    }

}
