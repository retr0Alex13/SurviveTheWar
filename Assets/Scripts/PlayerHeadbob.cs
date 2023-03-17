using StarterAssets;
using UnityEngine;

namespace OM
{
    public class PlayerHeadbob : MonoBehaviour
    {
        [Header("PlayerHeadbob Settings")]
        [SerializeField, Tooltip("Can player use head bob")]
        private bool canUseHeadBob = true;

        [SerializeField, Tooltip("Walk speed of the headbob effect")]
        private float walkBobSpeed = 12f;

        [SerializeField, Tooltip("Amount of the headbob effect when walking")]
        private float walkBobAmount = 0.04f;

        [SerializeField, Tooltip("Sprint speed of the headbob effect")]
        private float sprintBobSpeed = 15f;

        [SerializeField, Tooltip("Amount of the headbob effect when walking")]
        private float sprintBobAmount = 0.11f;

        [SerializeField] private GameObject playerCameraRoot;
        [SerializeField] private GameObject mainCamera;
        private StarterAssetsInputs playerInputs;
        private CharacterController characterController;
        private float headBobTimer;

        // used to reset camera position in headbob effect
        private float defaultYPos = 0;

        private void Awake()
        {
            playerInputs = GetComponent<StarterAssetsInputs>();
            characterController = GetComponent<CharacterController>();
            // get a reference to our player root gameObject
            if (playerCameraRoot == null)
            {
                playerCameraRoot = GameObject.FindGameObjectWithTag("CinemachineTarget");
            }
            defaultYPos = mainCamera.transform.localPosition.y;
        }

        private void Update()
        {
            HandleHeadBob();
        }

        private void HandleHeadBob()
        {
            if (!canUseHeadBob) return;
            if (!characterController.isGrounded) return;

            if (playerInputs.GetMove() != Vector2.zero)
            {
                headBobTimer += Time.deltaTime * (playerInputs.IsSprinting() ? sprintBobSpeed : walkBobSpeed);
                playerCameraRoot.transform.localPosition = new Vector3(
                    playerCameraRoot.transform.localPosition.x,
                    defaultYPos + Mathf.Sin(headBobTimer) * (playerInputs.IsSprinting() ? sprintBobAmount : walkBobAmount),
                    playerCameraRoot.transform.localPosition.z);
            }
            else
            {
                playerCameraRoot.transform.localPosition = new Vector3(playerCameraRoot.transform.localPosition.x, defaultYPos, playerCameraRoot.transform.localPosition.z);
            }
        }
    }
}