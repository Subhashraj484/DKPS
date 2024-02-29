using InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public override void ActivateObject()
    {
        this.AddWeapon();
    }

    private void AddWeapon()
    {
        CombatSystem.instance.EnableInventory();
        this.DestroyObject(0);
    }
}
