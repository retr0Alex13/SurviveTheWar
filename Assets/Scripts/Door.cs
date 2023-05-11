using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool isDoorClosed;
        private Animator animator;

        public bool IsPickable { get; set;}

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        [ContextMenu("Open/CloseDoor")]
        public void Interact()
        {
            isDoorClosed = !isDoorClosed;
            animator.SetBool("IsOpened", isDoorClosed);
        }
    }
}
