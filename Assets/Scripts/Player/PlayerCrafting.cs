using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

namespace OM
{
    public class PlayerCrafting : MonoBehaviour
    {
        StarterAssetsInputs playerInput;
        [SerializeField] private float interactDistance = 2f;
        [SerializeField] private LayerMask interactLayerMask;
        [SerializeField] private Transform playerCameraTransform;

        private void Awake()
        {
            playerInput = GetComponent<StarterAssetsInputs>();
        }

        private void Update()
        {
            if (playerInput.IsInteracting())
            {
                HandleCrafting();
            }
        }

        private void HandleCrafting()
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, interactDistance, interactLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out CraftingStation craftingWorkbench))
                {
                    if (playerInput.IsPickingup())
                    {
                        craftingWorkbench.NextRecipe();
                    }
                    if (playerInput.IsInteracting())
                    {
                        craftingWorkbench.Craft();
                    }
                }
            }
        }
    }
}