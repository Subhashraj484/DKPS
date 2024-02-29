using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;


public class Scale : Interactable
{
    bool IsOpen = false;
    public override void Interact()
    {
        //transform.localScale = Vector3.one*5f;
        transform.DORotate(new Vector3(0, 90, 0), 1f, RotateMode.Fast);
    }
}

