using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{

    public class PlayerCrafting : MonoBehaviour
    {
        StarterAssetsInputs playerInput;
        [SerializeField] private float interactDistance = 2f;
        [SerializeField] private LayerMask interactLayerMask;
        [SerializeField] private Transform playerCameraTransform;
        [SerializeField] private float coolDownTimer;

        private float coolDown = 0.5f;

        private void Awake()
        {
            playerInput = GetComponent<StarterAssetsInputs>();
        }

        private void Update()
        {
            if (coolDownTimer > 0)
            {
                coolDownTimer -= Time.deltaTime;
            }

            if (coolDownTimer < 0)
            {
                coolDownTimer = 0;
            }

            if (playerInput.IsInteracting() && coolDownTimer == 0)
            {
                HandleCrafting();
                coolDownTimer = coolDown;
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