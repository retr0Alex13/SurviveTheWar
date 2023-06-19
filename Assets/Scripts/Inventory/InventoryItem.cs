using System;
using System.Diagnostics;

namespace OM
{
    [Serializable]
    public class InventoryItem
    {
        public ItemSO itemData { get; set; }

        public int StackSize { get; set; }

        public int MaxStackSize { get; set; }

        public ItemDurability ItemDurability { get; set; }


        public InventoryItem(ItemSO source, ItemDurability itemDurability)
        {
            itemData = source;
            MaxStackSize = source.MaxStackSize;

            if (itemDurability != null)
            {
                ItemDurability = itemDurability;
            }
            else 
            {
                ItemDurability = null;
            }
            AddToStack();
        }

        public void AddToStack()
        {
            StackSize++;
        }

        public void RemoveFromStack() 
        {
            StackSize--;
        }
    }
}
