using UnityEngine;

namespace OM
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction settings")]
        [SerializeField, Tooltip("Distance for player interaction")]
        private float interactDistance = 4f;
        private int ineteractLayerMask = 1 << 10;

        // for storing interactable interface
        private Transform selection;

        void Update()
        {
            HandleInteraction();
        }

        private void HandleInteraction()
        {
            // if selecttion was setted
            if (selection != null)
            {
                if (selection.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Dehighlight();
                }
                // set selection to null so we don't deselect it
                selection = null;
            }

            var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance, ineteractLayerMask))
            {
                // get a hit transform
                Transform selected = hit.transform;
                if (hit.transform.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Highlight();
                }
                // set selection tranform to hit interface transofm
                selection = selected;
            }
        }
    }
}
