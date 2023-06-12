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
            
            if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 1.5f))
            {
                if (playerController.IsSprinting())
                {
                    switch (hit.collider.tag)
                    {
                        case "Footsteps/Grass":
                            SoundManager.Instance.PlaySound("GrassRun");
                            break;
                        case "Footsteps/Tile":
                            SoundManager.Instance.PlaySound("TileRun");
                            break;
                        case "Footsteps/Gravel":
                            SoundManager.Instance.PlaySound("GravelRun");
                            break;
                        case "Footsteps/Stone":
                            SoundManager.Instance.PlaySound("StoneRun");
                            break;
                            // default:
                            //     SoundManager.Instance.PlaySound("GravelRun");
                            //     break;
                    }
                }
                else
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
                        case "Footsteps/Gravel":
                            SoundManager.Instance.PlaySound("GravelFootsteps");
                            break;
                        case "Footsteps/Stone":
                            SoundManager.Instance.PlaySound("StoneFootsteps");
                            break;
                            // default:
                            //     SoundManager.Instance.PlaySound("GravelFootsteps");
                            //     break;
                    }
                }
            }
        }
    }
}
