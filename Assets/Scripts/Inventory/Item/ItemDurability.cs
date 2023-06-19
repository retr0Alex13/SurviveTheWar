using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class ItemDurability : MonoBehaviour
    {
        [SerializeField] private float currentDurability;
        [SerializeField] private float maxDurability;
        [SerializeField] private ItemSO itemSO;

        public ItemSO ItemSO { get { return itemSO; } }

        public float MaxDurability { get { return maxDurability; } }

        public float CurrentDurability
        {
            get { return currentDurability; }
            set
            {
                if (value > maxDurability)
                {
                    currentDurability = maxDurability;
                }
                else
                {
                    currentDurability = value;
                }
            }
        }

        private void Start()
        {
            //currentDurability = maxDurability;
            itemSO = GetComponent<ItemSOHolder>().ItemSO;
        }

        public void SetComponent(ItemDurability itemDurability)
        {
            currentDurability = itemDurability.CurrentDurability;
            maxDurability = itemDurability.MaxDurability;
            itemSO = itemDurability.ItemSO;
        }

        public void SetDurability(float amount)
        {
            currentDurability = amount;

            if (currentDurability > maxDurability)
            {
                currentDurability = maxDurability;
            }
        }

        public void AddDurability(float amount)
        {
            currentDurability += amount;

            if (currentDurability > maxDurability)
            {
                currentDurability = maxDurability;
            }
        }

        public void DecreaseDurability(float amount)
        {
            currentDurability -= amount;
        }
    }
}
