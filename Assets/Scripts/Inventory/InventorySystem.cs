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

        public void Add(ItemSO referenceData, ItemDurability itemDurability)
        {
            if (ItemDictionary.TryGetValue(referenceData, out InventoryItem value))
            {
                value.AddToStack();
            }
            else
            {
                if(itemDurability == null)
                {
                    CreateNewItemCell(referenceData, null);
                    Debug.Log("ItemDurability is null");
                }
                else
                {
                    CreateNewItemCell(referenceData, itemDurability);
                }
            }
            SoundManager.Instance.PlaySound("PickupItem");
            OnInventoryChange();
        }

        private void CreateNewItemCell(ItemSO referenceData, ItemDurability itemDurability)
        {
            if (itemDurability == null)
            {
                InventoryItem newItem = new InventoryItem(referenceData, null);
                Inventory.Add(newItem);
                ItemDictionary.Add(referenceData, newItem);
                Debug.Log("Item durability is null");
            }
            else
            {
                InventoryItem newItem = new InventoryItem(referenceData, itemDurability);
                Inventory.Add(newItem);
                ItemDictionary.Add(referenceData, newItem);
                Debug.Log(newItem);
                Debug.Log("Item durability in new cell: " + newItem.ItemDurability.CurrentDurability);
            }
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
