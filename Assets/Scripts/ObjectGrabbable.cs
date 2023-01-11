using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    [SerializeField] float lerpSpeed = 10f;
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;
    private bool isInTexture = false;

    private void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
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

    private void OnCollisionEnter(Collision collision)
    {
        isInTexture = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isInTexture = false;
    }
}
