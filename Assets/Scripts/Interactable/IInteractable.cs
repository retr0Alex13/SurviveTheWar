using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public interface IInteractable
    {
        public bool IsPickable { get; set; }
        void Interact();
    }
}
