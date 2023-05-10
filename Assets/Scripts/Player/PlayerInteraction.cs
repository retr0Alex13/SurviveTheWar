using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction settings")]
        [SerializeField, Tooltip("Distance for player interaction")]
        private float interactDistance = 4f;
        private int ineteractLayerMask = 1 << 10;
        [SerializeField] private GameObject hintPanel;
        [SerializeField] private TextMeshProUGUI hintText;

        private PlayerInput playerInput;

        // for storing selectable interface
        private Transform selection;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        void Update()
        {
            HandleSelection();
        }

        private void HandleSelection()
        {
            // if selecttion was setted
            if (selection != null)
            {
                if (selection.TryGetComponent(out ISelectable interactable))
                {
                    interactable.Dehighlight();
                }
                // set selection to null so we don't deselect it
                selection = null;
            }

            RaycastHit hit;
            Ray ray = FireRay();
            var pickUpKey = playerInput.actions["Pickup"].GetBindingDisplayString();
            var interactKey = playerInput.actions["Interact"].GetBindingDisplayString();

            if (Physics.Raycast(ray, out hit, interactDistance, ineteractLayerMask))
            {
                // get a hit transform
                Transform selected = hit.transform;
                if (hit.transform.TryGetComponent(out ISelectable selectable))
                {
                    selectable.Highlight();
                }
                if (hit.transform.TryGetComponent(out IInteractable interactable))
                {
                    if (interactable.IsPickable)
                    {
                        hintText.text = $"Натисність \"{pickUpKey}\" щоб підібрати";
                        hintPanel.SetActive(true);
                    }
                    else
                    {
                        hintText.text = $"Натисність \"{interactKey}\" щоб взаємодіяти";
                        hintPanel.SetActive(true);
                    }
                }
               
                // set selection tranform to hit interface transofm
                selection = selected;
            }
            else
            {
                hintPanel.SetActive(false);
            }
        }

        private static Ray FireRay()
        {
            return Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                RaycastHit hit;
                Ray ray = FireRay();
                if (Physics.Raycast(ray, out hit, interactDistance, ineteractLayerMask))
                {
                    if (hit.transform.TryGetComponent(out IInteractable interactable))
                    {
                        interactable.Interact();
                    }
                }
            }
        }
    }
}
