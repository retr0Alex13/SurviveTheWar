using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class Flashlight : MonoBehaviour, IInteractable
    {
        private Transform sourceLight;

        public bool IsPickable { get; set; }

        public void Interact()
        {
            if (transform.GetChild(0) == null)
                return;
            sourceLight = transform.GetChild(0);
            sourceLight.gameObject.SetActive(!sourceLight.gameObject.activeSelf);
        }
    }
}
