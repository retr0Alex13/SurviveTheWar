using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class Flashlight : MonoBehaviour, IInteractable
    {
        private Transform light;
        public void Interact()
        {
            light = transform.GetChild(0);
            light.gameObject.SetActive(!light.gameObject.activeSelf);
        }
    }
}
