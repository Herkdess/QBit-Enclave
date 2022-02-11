using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base.UI;
using RPGSystems;
using UnityEngine;
public class PlayerMain : RPGSystemUser {
    

    protected override void Awake() {
        
        base.Awake();
    }

    private IEnumerator Start() {
        yield return new WaitForSeconds(.2f);
        int Timer = 1;
        while (Stats.Health.Value.Value > 0) {
            yield return new WaitForSeconds(.5f);
            TakeDamage(Stats.AttackPower.Value.Value);
            Timer++;
            if (Timer % 2 == 0) {
                if (Storage.inventoryObject.inventorySlots[0] == null) {
                    //Add a system for it to remove equiped items
                    
                    // AddItem(Equipment.inventoryObject.inventorySlots.Where(t => !t.IsEmpty).ToList()[0].itemObject);
                    Debug.Log("Slot Is Empty");
                    yield break;
                }
                EquipItem(Storage.inventoryObject.inventorySlots.Where(t => t.IsEmpty == false).ToList()[0].itemObject);
            }
        }
    }

    protected override void OnDeath() {
        base.OnDeath();
    }

    public override void TakeDamage(float value) {
        base.TakeDamage(value);
    }

    protected override void OnDisable() {
        base.OnDisable();
    }
}