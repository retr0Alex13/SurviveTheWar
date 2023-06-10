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
        public bool IsPickable { get; set; }

        public delegate void GasolineAction();

        public static event GasolineAction OnGasolineUsed;
        
        public void Interact()
        {
            fuelAmount = transform.GetComponent<ItemSOHolder>().ItemCapacity;
            
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
                    generatorController.AddFuel(fuelAmount);
                    OnGasolineUsed?.Invoke();
                    fuelAmount = 0;
                }
            }

        }
    }
}
