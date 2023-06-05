using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class Gasoline : MonoBehaviour, IInteractable
    {
        [SerializeField] private float fuelAmount = 50f;
        [SerializeField] private float fillingDistance = 1.5f;
        private Transform playerCameraTransform;
        public bool IsPickable { get; set; }
        
        public void Interact()
        {
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
                    fuelAmount = 0;
                }
            }

        }
    }
}
