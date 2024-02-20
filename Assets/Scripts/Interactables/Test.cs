using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Test : Interactable
{
    public override void Interact()
    {
        GetComponent<Renderer>().material.color = Color.black;
    }
}

