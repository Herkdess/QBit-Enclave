using System;
using Base;
using Sirenix.Utilities;
using UnityEngine;
namespace RPGSystems {
    public abstract class RPGSystemUser : MonoBehaviour {

        public Inventory Storage;
        public Inventory Equipment;
        public RPGStatBase Stats;

        public B_ATModifier DamageTaken;


        protected virtual void Awake() {
            Stats.InitStats();
            Stats.LoadModifiedData();
            InitInventories();
            DamageTaken = new B_ATModifier(0, AT_AttributeModifierType.Flat);
            Stats.Health.Value.AddModifier(DamageTaken);
        }

        void InitInventories() {
            Equipment.inventoryObject.inventorySlots.ForEach(t => t.ItemEquipped += Stats.EquipItem);
            Equipment.inventoryObject.inventorySlots.ForEach(t => t.ItemUnequipped += Stats.RemoveItem);
            // Storage.inventoryObject.inventorySlots.ForEach(t => t.ItemAdded += Stats.RemoveItem);
            // Storage.inventoryObject.inventorySlots.ForEach(t => t.ItemRemoved += Stats.RemoveItem);
            Equipment.InitInventory();
            Storage.InitInventory();
        }

        void FlushInventories() {
            Storage.FlushInventory();
            Equipment.FlushInventory();
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

        protected virtual void AddItem(Item itemObject) {
            if (Storage.inventoryObject.AddItem(itemObject)) { }
        }

        protected virtual void EquipItem(Item itemObject) {
            //First check if can equip, if can, send signals to where its necessary
            if (!Stats.CanEquipItem(itemObject.item)) return;
            if (Equipment.inventoryObject.AddItem(itemObject)) {
                Stats.EquipItem(itemObject);
            }

            Debug.Log("Equipped item");
            //If it can't use the item, send another signal
        }

        protected virtual void RemoveItem(Item itemObject) {
            Storage.inventoryObject.RemoveItem(itemObject);
        }

        protected virtual void UnequipItem(Item itemObject) {
            Stats.RemoveItem(itemObject);
        }

        protected virtual void OnDeath() {
            Debug.Log("Died");
        }

        protected virtual void OnDisable() {
            Stats.Health.Value.RemoveModifier(DamageTaken);
            FlushInventories();
            Stats.SaveModifiedData();
            Stats.LoadOriginalData();
        }
    }
}