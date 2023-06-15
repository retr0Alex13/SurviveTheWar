using System;

namespace OM
{
    [Serializable]
    public class InventoryItem
    {
        public ItemSO itemData { get; set; }

        public int StackSize { get; set; }

        public int MaxStackSize { get; set; }

        public InventoryItem(ItemSO source)
        {
            itemData = source;
            MaxStackSize = source.MaxStackSize;
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
