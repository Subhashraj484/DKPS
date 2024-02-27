using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Scale : Interactable
{
    public override void Interact()
    {
        transform.localScale = Vector3.one*5f;
    }
}

