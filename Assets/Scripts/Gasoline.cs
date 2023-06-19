using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class Gasoline : MonoBehaviour, IInteractable
    {
        [SerializeField] private float fillingDistance = 1.5f;

        private float fuelAmount;

        private Transform playerCameraTransform;

        [SerializeField] private ItemDurability itemDurability;

        public bool IsPickable { get; set; }

        public delegate void GasolineAction();

        public static event GasolineAction OnGasolineUsed;
        
        public void Interact()
        {
            itemDurability = gameObject.GetComponent<ItemDurability>();

            foreach (Transform obj in transform.root)
            {
                playerCameraTransform = obj.parent;
            }
            
            if(playerCameraTransform == null)
                return;

            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
                    out RaycastHit raycastHit, fillingDistance))
            {
                if (raycastHit.transform.TryGetComponent(out PowerGeneratorController generatorController))
                {
                    fuelAmount = itemDurability.CurrentDurability;
                    generatorController.AddFuel(fuelAmount);
                    OnGasolineUsed?.Invoke();
                    fuelAmount = 0;
                }
            }

        }
    }
}
