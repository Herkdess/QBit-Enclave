using System;
using System.Linq;
using Base;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos.RPGEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;
namespace RPGSystems {
    public enum InventoryType { Storage, Equipment }
    [System.Serializable]
    public class InventoryObject {
        [OnValueChanged("ChangeType")]
        public InventoryType type;
        [ShowInInspector]
        public InventorySlot[] inventorySlots;

        public InventoryObject() {
            type = InventoryType.Storage;
            inventorySlots = new InventorySlot[24];
        }

        public void InitInventory() {
            inventorySlots.ForEach(t => t.InitSlot());
        }

        public void FlushInventory() {
            inventorySlots.ForEach(t => t.FlushSlot());
        }

        public bool AddItem(Item itemObject) {
            switch (type) {
                case InventoryType.Storage:
                    //Check if the slot is empty
                    if (!HasEmptySlot()) {
                        //if not, dump item
                        Debug.Log("Has No Empty Slots");
                        return false;
                    }
                    //If the slot empty, add item to the first empty slot
                    inventorySlots.Where(t => t.IsEmpty).ToList().First().AddItem(itemObject);
                    return true;

                case InventoryType.Equipment:
                    //First, decide if its the right slot
                    InventorySlot slot = inventorySlots.Where(t => t.type == itemObject.item.slotType).ToList()[0];
                    
                    return slot.AddItem(itemObject);
                default:
                    return false;
            }
            return false;
        }

        public void RemoveItem(Item item) { }

        public void ChangeType() {
            switch (type) {

                case InventoryType.Storage:
                    inventorySlots = new InventorySlot[24];
                    break;
                case InventoryType.Equipment:
                    inventorySlots = new InventorySlot[] {
                        new InventorySlot(SlotType.Head),
                        new InventorySlot(SlotType.Torso),
                        new InventorySlot(SlotType.Legs),
                        new InventorySlot(SlotType.Hands1),
                        new InventorySlot(SlotType.Hands2),
                    };
                    break;
            }
        }

        bool HasEmptySlot() {
            return inventorySlots.Any(t => t.IsEmpty);
        }

    }

    public enum SlotType { Head, Torso, Legs, Hands1, Hands2, Storage }
    [Serializable]
    public class InventorySlot {
        public Action<Item> ItemRemoved;
        public Action<Item> ItemAdded;
        public Action<Item> ItemEquipped;
        public Action<Item> ItemUnequipped;
        // public Action<Item> OnInventoryChanged;
        [HorizontalGroup("Split")]
        [VerticalGroup("Split/Left")]
        [HideLabel, LabelWidth(50)]
        public SlotType type;
        [VerticalGroup("Split/Right"), HideLabel]
        public Item itemObject;

        public bool IsEmpty => itemObject == null;

        public InventorySlot() {
            type = SlotType.Storage;
            itemObject = null;
        }

        public InventorySlot(SlotType slotType) {
            type = slotType;
            itemObject = null;
        }

        public void InitSlot() {
            if (!IsEmpty) {
                if (type == SlotType.Storage) {
                    itemObject.AddItem(this);
                    ItemAdded?.Invoke(itemObject);
                    return;
                }
                itemObject.EquipItem(this);
                ItemEquipped?.Invoke(itemObject);
            }
        }

        public void FlushSlot() {
            if (!IsEmpty) {
                ItemRemoved?.Invoke(itemObject);
                if (type == SlotType.Storage) {
                    itemObject.RemoveItem(this);
                    ItemRemoved?.Invoke(itemObject);
                    ItemRemoved = null;
                    ItemAdded = null;
                    return;
                }
                itemObject.UnequipItem(this);
                ItemUnequipped?.Invoke(itemObject);
                ItemRemoved = null;
                ItemAdded = null;
            }
        }

        public bool AddItem(Item itemObject) {
            //Check the slot type first, if its storage, it will add, because check for it happens on the Inventory
            if (type == SlotType.Storage) {
                if (itemObject.Equipped) {
                    itemObject.UnequipItem(this);
                }
                //First set the slot to new item
                this.itemObject = itemObject;
                //then change the owner of the item
                itemObject.AddItem(this);
                Debug.Log(itemObject._inventorySlot == null);
                //Finally send a signal for UI or other things
                ItemAdded?.Invoke(this.itemObject);
                Debug.Log("Item Added To The Inventory");
                return true;
            }
            //Check if the slot is empty
            if (!IsEmpty) { 
                //If slot is not empty, replace the items
                Debug.Log("Slot is not empty, so we are replacing items");
                //If its not empty, first, save the slot
                InventorySlot slot = itemObject._inventorySlot;
                //then remove it from that slot
                slot.RemoveItem();
                //Then add the old item back to the users storage
                slot.AddItem(this.itemObject);
                //Now we can empty this slot
                RemoveItem();
                //Set the new Item, and then send signal
                this.itemObject = itemObject;
                itemObject.EquipItem(this);
                ItemEquipped?.Invoke(itemObject);
                return true;
            }
            //If everything is fine, set item and send signals
            itemObject._inventorySlot?.RemoveItem();
            this.itemObject = itemObject;
            itemObject.EquipItem(this);
            ItemEquipped?.Invoke(itemObject);
            Debug.Log("Item can be added");
            return true;
        }

        public void RemoveItem() {
            if (type == SlotType.Storage) {
                this.ItemRemoved?.Invoke(this.itemObject);
                this.itemObject.RemoveItem(this);
                this.itemObject = null;
                return;
            }
            ItemUnequipped?.Invoke(itemObject);
            this.itemObject.UnequipItem(this);
            this.itemObject = null;
        }

    }
}