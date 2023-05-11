using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class Footsteps : MonoBehaviour
    {
        [SerializeField] private Transform playerCamera;
        private FirstPersonController playerController;

        private void Awake()
        {
            playerController = GetComponent<FirstPersonController>();
        }

        private void Update()
        {
            if(!playerController.IsGrounded()) return;
            if (!playerController.IsMoving()) return;
            
            if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 5f))
            {
                switch (hit.collider.tag)
                {
                    case "Footsteps/Grass":
                        SoundManager.Instance.PlaySound("GrassFootsteps");
                        break;
                    case "Footsteps/Tile":
                        SoundManager.Instance.PlaySound("TileFootsteps");
                        break;
                    case "Footsteps/Wood":
                        SoundManager.Instance.PlaySound("WoodFootsteps");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
