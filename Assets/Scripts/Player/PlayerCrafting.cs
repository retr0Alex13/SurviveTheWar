using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

namespace OM
{
    public class PlayerCrafting : MonoBehaviour
    {
        [SerializeField] private float interactDistance = 2f;
        [SerializeField] private float interactionCooldown = 0.5f; // Set the cooldown time here
        [SerializeField] private LayerMask interactLayerMask;
        [SerializeField] private Transform playerCameraTransform;
        StarterAssetsInputs playerInput;
        private bool canInteract = true; // Use this to disable the input while the coroutine is running

        private void Awake()
        {
            playerInput = GetComponent<StarterAssetsInputs>();
        }

        private void Update()
        {
            if (canInteract && playerInput.IsInteracting())
            {
                StartCoroutine(HandleCrafting());
            }
        }

        private IEnumerator HandleCrafting()
        {
            canInteract = false;
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, interactDistance, interactLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out CraftingStation craftingWorkbench))
                {
                    if (playerInput.IsInteracting())
                    {
                        craftingWorkbench.NextRecipe();
                        yield return new WaitForSeconds(interactionCooldown); // Wait for the cooldown time
                    }
                    if (playerInput.IsPickingup())
                    {
                        craftingWorkbench.Craft();
                    }
                }
            }
            canInteract = true; // Enable input again
        }
    }
}