using UnityEngine;

namespace OM
{
    public class InventoryMediator : MonoBehaviour, IInventoryMediator
    {
        [SerializeField] private InventoryUI inventoryUI;
        private Transform inventoryPanel;

        private void Start()
        {
            inventoryPanel = inventoryUI.gameObject.GetComponent<Transform>();
        }

        public void FireEvent(bool isIinventoryChanged)
        {
            if(isIinventoryChanged)
            {
                inventoryUI.UpdateInventoryUI(inventoryPanel);
            }
        }
    }
}
