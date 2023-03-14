using UnityEngine;

namespace OM
{
    public class ObjectGrabbable : MonoBehaviour, IInteractable
    {
        [SerializeField] private float lerpSpeed = 10f;
        private Rigidbody objectRigidBody;
        private Transform objectGrabPointTransform;
        private Outline outline;
        private bool isInTexture = false;

        private void Awake()
        {
            objectRigidBody = GetComponent<Rigidbody>();
            outline = GetComponent<Outline>();
        }

        public void Grab(Transform objectGrabPointTransform)
        {
            this.objectGrabPointTransform = objectGrabPointTransform;
            objectRigidBody.useGravity = false;
            objectRigidBody.isKinematic = true;
        }

        public void Drop()
        {
            if (isInTexture) return;
            this.objectGrabPointTransform = null;
            objectRigidBody.useGravity = true;
            objectRigidBody.isKinematic = false;
        }

        private void FixedUpdate()
        {
            if (objectGrabPointTransform != null)
            {

                Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
                objectRigidBody.MovePosition(newPosition);
            }
        }

        private void Update()
        {
            if (objectGrabPointTransform != null)
            {
                transform.rotation = Quaternion.Euler(0f, objectGrabPointTransform.eulerAngles.y, 0f);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            isInTexture = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            isInTexture = false;
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