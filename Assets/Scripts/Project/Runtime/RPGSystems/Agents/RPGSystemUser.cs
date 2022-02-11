using System;
using Base;
using Sirenix.Utilities;
using UnityEngine;
namespace RPGSystems {
    public abstract class RPGSystemUser : MonoBehaviour {
        
        public RPGStatBase Stats;
        public InventoryObject inventoryObject;
        public B_ATModifier DamageTaken;


        protected virtual void Awake() {
            inventoryObject.load();
            Stats.InitStats();
            Stats.LoadModifiedData();
            DamageTaken = new B_ATModifier(0, AT_AttributeModifierType.Flat);
            Stats.Health.Value.AddModifier(DamageTaken);
            inventoryObject.load();
        }

        public virtual void TakeDamage(float value) {
            if (Stats.Health.Value.Value <= 0) return;
            DamageTaken.Value -= value;
            if (Stats.Health.Value.Value <= 0) {
                OnDeath();
                return;
            }
            Debug.Log(Stats.Health.Value.Value);
        }
        

        protected virtual void OnDeath() {
            Debug.Log("Died");
        }

        protected virtual void OnDisable() {
            Stats.Health.Value.RemoveModifier(DamageTaken);
            //For testing
            // inventoryObject.Container.Clear();
            inventoryObject.save();
            
            Stats.SaveModifiedData();
            Stats.LoadOriginalData();
        }
    }
}