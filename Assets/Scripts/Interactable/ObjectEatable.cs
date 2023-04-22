using UnityEngine;

namespace OM
{
    public class ObjectEatable : MonoBehaviour, IInteractable
    {
        public delegate void EatableAction(float food, float water);
        public static event EatableAction OneItemConsuming;

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

        public void Consume()
        {
            OneItemConsuming?.Invoke(FoodToRestore, ThirstToRestore);
        }
    }
}