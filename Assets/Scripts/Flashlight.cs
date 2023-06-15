using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class Flashlight : MonoBehaviour, IInteractable
    {
        private Transform sourceLight;
        private float currentCharge;

        public bool IsPickable { get; set; }

        public void Interact()
        {
            if (transform.GetChild(0) == null)
                return;

            ItemSOHolder itemSOHolder = transform.GetComponent<ItemSOHolder>();

            currentCharge = itemSOHolder.ItemCapacity;

            if (currentCharge <= 0)
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


            if (currentCharge <= 0)
            {
                sourceLight.gameObject.SetActive(false);
                currentCharge = 0;
                return;
            }

            if (sourceLight.gameObject.activeSelf)
            {
                currentCharge -= Time.deltaTime;
                Debug.Log(currentCharge);
            }

            transform.GetComponent<ItemSOHolder>().ItemCapacity = currentCharge;
        }
    }
}
