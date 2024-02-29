using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseArea : MonoBehaviour
{
    public AudioSource chaseSound;

    private void Start()
    {
        chaseSound.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            chaseSound.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            chaseSound.Stop();
        }
    }
}
