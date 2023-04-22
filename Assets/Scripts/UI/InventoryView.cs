using UnityEngine;

namespace OM
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private GameObject itemSlotPrefab;
        [SerializeField] public InventoryMediator inventoryMediator;

        public void OnUpdateInventory()
        {
            foreach(Transform t in transform)
            {
                Destroy(t.gameObject);
            }

            DrawInventory();
        }

        public void DrawInventory()
        {
            foreach (InventoryItem item in inventoryMediator.InventorySystem.Inventory)
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