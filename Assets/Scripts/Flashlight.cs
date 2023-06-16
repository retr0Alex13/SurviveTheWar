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

        public float GetDurability()
        {
            throw new System.NotImplementedException();
        }

        public void Interact()
        {
            if (transform.GetChild(0) == null)
                return;

            if (currentCharge <= 0)
            {
                return;
            }

            sourceLight = transform.GetChild(0);
            sourceLight.gameObject.SetActive(!sourceLight.gameObject.activeSelf);
            SoundManager.Instance.PlaySound("FlashlightClick");
        }

        public void SetDurability(float currentDurability)
        {
            Debug.Log("Setted flashlight charge: " + currentCharge);
            currentCharge = currentDurability;
            GetComponent<ItemSOHolder>().CurrentDurability = currentDurability;
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
                GetComponent<ItemSOHolder>().CurrentDurability = currentCharge;
                Debug.Log(currentCharge);
            }
        }
    }
}
