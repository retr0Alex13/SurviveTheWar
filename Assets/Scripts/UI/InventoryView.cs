using System.Linq;
using UnityEngine;

namespace OM
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] public GameObject itemSlotPrefab;
        [SerializeField] public InventoryMediator inventoryMediator;

        public void OnUpdateInventory()
        {
            foreach(Transform t in transform)
            {
                Destroy(t.gameObject);
            }

            DrawInventory();
        }

        public ItemSlot GetItemSlot(ItemSO itemso)
        {
            foreach (Transform item in transform)
            {
                if (item.GetComponent<ItemSOHolder>().ItemSO == itemso)
                {
                    return item.GetComponent<ItemSlot>();
                }
            }
            return null;
        }

        public void DrawInventory()
        {
            foreach (InventoryItem item in inventoryMediator.InventorySystem.Inventory.ToList())
            {
                AddInventorySlot(item);
            }
        }

        public void AddInventorySlot(InventoryItem item)
        {
            GameObject obj = Instantiate(itemSlotPrefab);
            obj.transform.SetParent(transform, false);

            ItemSlot itemSlot = obj.GetComponent<ItemSlot>();
            itemSlot.Set(item);
        }
    }
}