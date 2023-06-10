using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace OM
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool isOpened;
        private Animator animator;

        public bool IsPickable { get; set; }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            isOpened = false;
        }

        [ContextMenu("Open/CloseDoor")]
        public void Interact()
        {
            isOpened = !isOpened;
            animator.SetBool("isOpened", isOpened);
        }

        public void PlayCloseDoorSound()
        {
            SoundManager.Instance.PlaySound("DoorClosing");
        }

        public void PlayOpenDoorSound()
        {
            SoundManager.Instance.PlaySound("DoorOpening");
        }
    }
}
        
