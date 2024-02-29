using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoM.Super;
using UnityEngine.Jobs;
using Unity.VisualScripting;

public class PickupAndDrop : SuperBehaviour 
{    
    [SerializeField] Camera mianCamera;
    [SerializeField] float pickupRange = 20f;
    [SerializeField] LayerMask pickupObjectLayer;
    [SerializeField] Transform objectHoldPoint;
    RaycastHit  raycastHit;
    bool pickup;
    Transform pickedObject;
    Rigidbody pickedRigidbody;
    float smoothTime = 0.2f;
    float throwForce = 10f;

    void Update()
    {
        Ray ray = mianCamera.ViewportPointToRay(new Vector3(0.5f , 0.5f , 0));

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(pickup)
            {
                ThrowObject();
            }
            else if(Physics.Raycast(ray.origin , ray.direction , out raycastHit, pickupRange , pickupObjectLayer ))
            {
                PickObject(); 
            }
        }

        if(pickedObject != null)
        {
            Vector3 targetPosition = Vector3.Lerp(pickedObject.position , objectHoldPoint.position , smoothTime);
            pickedRigidbody.MovePosition(targetPosition);
        }
    }

    void PickObject()
    {
        pickedObject = raycastHit.transform;
        if(pickedObject.TryGetComponent<Interactable>(out Interactable interactable))
        {
            interactable.Interact();
        }
        else
        {
            pickedRigidbody = pickedObject.transform.GetComponent<Rigidbody>();
            pickedRigidbody.useGravity = false;
            pickup = true;
        }
    }

    void ThrowObject()
    {
        if(pickedObject != null && pickedRigidbody != null)
        {
            pickedRigidbody.useGravity = true;
            pickedRigidbody.AddForce(mianCamera.transform.forward * throwForce, ForceMode.Impulse);
            pickedObject = null;
            pickedRigidbody = null;
            pickup = false;
        }
    }
}
