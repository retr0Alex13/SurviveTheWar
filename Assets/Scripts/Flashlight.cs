using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class Flashlight : MonoBehaviour, IInteractable
    {
        private Transform sourceLight;

        private ItemDurability itemDurability;
        public ItemDurability ItemDurability { get; }

        public bool IsPickable { get; set; }

        public void Interact()
        {
            itemDurability = gameObject.GetComponent<ItemDurability>();

            if (transform.GetChild(0) == null)
                return;

            if (itemDurability.CurrentDurability <= 0)
            {
                return;
            }

            sourceLight = transform.GetChild(0);
            sourceLight.gameObject.SetActive(!sourceLight.gameObject.activeSelf);
            SoundManager.Instance.PlaySound("FlashlightClick");
        }

        private void Update()
        {
            if (sourceLight == null)
                return;

            if (itemDurability.CurrentDurability <= 0)
            {
                sourceLight.gameObject.SetActive(false);
                itemDurability.CurrentDurability = 0;
                return;
            }

            if (sourceLight.gameObject.activeSelf)
            {
                itemDurability.CurrentDurability -= Time.deltaTime;
                Debug.Log(itemDurability.CurrentDurability);
            }
        }
    }
}
