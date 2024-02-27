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
    void Update()
    {
        Ray ray = mianCamera.ViewportPointToRay(new Vector3(0.5f , 0.5f , 0));

        if(Input.GetKeyDown(KeyCode.F) && pickup)
            {
                if(pickedObject != null)
                {
                    pickedRigidbody.useGravity = true;
                    pickedObject = null;
                    pickedRigidbody = null;
                    pickup = false;
                    //trow
                }
            }
            else

        if(Physics.Raycast(ray.origin , ray.direction , out raycastHit, pickupRange , pickupObjectLayer ) && !pickup)
        {
            if(Input.GetKeyDown(KeyCode.F) )
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
            
        }

        if(pickedObject != null)
        {
            Vector3 targetPosition = Vector3.Lerp(pickedObject.position , objectHoldPoint.position , smoothTime);
            pickedRigidbody.MovePosition(targetPosition);
        }
        
    }
}

