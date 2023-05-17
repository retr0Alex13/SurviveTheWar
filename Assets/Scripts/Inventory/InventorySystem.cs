using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class InventorySystem
    {
        public int slotsLimit = 6;
        public Dictionary<ItemSO, InventoryItem> ItemDictionary;
        public List<InventoryItem> Inventory { get; set; }

        public delegate void InventoryAction();
        public static event InventoryAction OnInventoryChange;


        public InventoryItem Get(ItemSO item)
        {
            if (ItemDictionary.TryGetValue(item, out InventoryItem inventoryItem))
            {
                return inventoryItem;
            }
            return null;
        }

        public void Add(ItemSO referenceData)
        {
            if (ItemDictionary.TryGetValue(referenceData, out InventoryItem value))
            {
                value.AddToStack();
            }
            else
            {
                CreateNewItemCell(referenceData);
            }
            SoundManager.Instance.PlaySound("PickupItem");
            OnInventoryChange();
        }

        private void CreateNewItemCell(ItemSO referenceData)
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            Inventory.Add(newItem);
            ItemDictionary.Add(referenceData, newItem);
        }

        public void Remove(ItemSO referenceData)
        {
            if (ItemDictionary.TryGetValue(referenceData, out InventoryItem value))
            {
                if (value.StackSize == 1)
                {
                    Inventory.Remove(value);
                    ItemDictionary.Remove(referenceData);
                }
                value.RemoveFromStack();
            }
            OnInventoryChange();
        }
    }
}
