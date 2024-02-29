using InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
    [Header("WEAPON PARAMETERS")]
    [Space(10)]
    [SerializeField] public bool hasMachete = false;
    [SerializeField] public GameObject handMachete;
    [SerializeField] public GameObject inventoryMachete;

    [Header("WEAPON SOUNDS")]
    [Space(10)]
    [SerializeField] private string pickUp;
    [SerializeField] private string getMachete;

    public static CombatSystem instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        handMachete.SetActive(false);
        inventoryMachete.SetActive(false);
        handMachete.GetComponent<Collider>().enabled = false;
    }

    public void EnableInventory()
    {
        hasMachete = true;
        inventoryMachete.SetActive(true);
        AudioManager.instance.Play(pickUp);
    }

    void EnableWeapon()
    {
        handMachete.SetActive(true);
        inventoryMachete.SetActive(false);
        AudioManager.instance.Play(getMachete);
    }

    void DisableWeapon()
    {
        handMachete.SetActive(false);
        inventoryMachete.SetActive(true);
        AudioManager.instance.Play(getMachete);
    }

    public void TriggerEnable()
    {
        handMachete.GetComponent<Collider>().enabled = true;
    }

    public void TriggerDisable()
    {
        handMachete.GetComponent<Collider>().enabled = false;
    }
}
