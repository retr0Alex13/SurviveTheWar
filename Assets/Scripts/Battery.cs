using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class Battery : MonoBehaviour, IInteractable
    {
        public bool IsPickable { get; set; }
        public Flashlight flashlight;

        private ItemDurability itemDurability;

        public void Interact()
        {
            itemDurability = gameObject.GetComponent<ItemDurability>();

            Debug.Log(itemDurability);

            if (flashlight == null)
                return;

            flashlight.gameObject.GetComponent<ItemDurability>().AddDurability(itemDurability.CurrentDurability);
        }
    }
}
