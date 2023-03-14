using UnityEngine;

namespace OM
{
    public class ObjectEatable : MonoBehaviour, IInteractable
    {
        [SerializeField] private float foodToRestore;
        [SerializeField] private float thirstToRestore;
        private Outline outline;

        private void Awake()
        {
            outline = GetComponent<Outline>();
        }

        public float FoodToRestore { get { return foodToRestore; } }
        public float ThirstToRestore { get { return thirstToRestore; } }

        private void Start()
        {

        }

        public void Highlight()
        {
            if (outline == null) return;
            outline.enabled = true;
        }

        public void Dehighlight()
        {
            if (outline == null) return;
            outline.enabled = false;
        }
    }
}