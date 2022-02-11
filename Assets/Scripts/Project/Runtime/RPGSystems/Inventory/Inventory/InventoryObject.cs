using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEditor;
namespace RPGSystems {

    [CreateAssetMenu(fileName = "New Inventory Object", menuName = "RPG/Inventory/New Inventory")]
    public class InventoryObject : SerializedScriptableObject {

        public ItemDatabaseObject DatabaseObject;
        public Inventory Container;

        public void AddItem(Item _item, int _amount) {
            if (!_item.Stackable) {
                SetFirstEmptySlot(_item, _amount);
                return;
            }
            
            for (int i = 0; i < Container.Items.Length; i++) {
                if (Container.Items[i].ID != _item.ID) continue;
                Container.Items[i].AddAmount(_amount);
                return;
            }
            
            SetFirstEmptySlot(_item, _amount);
        }

        
        public InventorySlot GetFirstEmptySlot() {
            return Container.Items.FirstOrDefault(t => t.ID <= -1);
        }
        
        //Set the first empty slot with the updateslot function with id, item and amount
        public InventorySlot SetFirstEmptySlot(Item _item, int _amount) {
            var slot = GetFirstEmptySlot();
            slot?.UpdateSlot(_item.ID, _item, _amount);
            //If the slot is null, setup a function to drop the inventory back on the ground
             
            return slot;
        }
        
        //Replace item with given another item
        public void ReplaceItem(Item _item, int _amount) {
            for (int i = 0; i < Container.Items.Length; i++) {
                if (Container.Items[i].ID != _item.ID) continue;
                Container.Items[i].UpdateSlot(_item.ID, _item, _amount);
                return;
            }
        }
        
        //A function that takes two items, and replaces the first item with the second item
        public void ReplaceItem(InventorySlot _item1, InventorySlot _item2) {
            InventorySlot temp = new InventorySlot(_item2.ID, _item2.Item, _item2.Amount);
            _item2.UpdateSlot(_item1.ID, _item1.Item, _item1.Amount);
            _item1.UpdateSlot(temp.ID, temp.Item, temp.Amount);
        }
        
        //Remove item from inventory
        public void RemoveItem(InventorySlot _item, int _amount) {
            for (int i = 0; i < Container.Items.Length; i++) {
                if (Container.Items[i].Item != _item.Item) continue;
                Container.Items[i].RemoveAmount(_amount);
                return;
            }
        }

        public void RemoveItem(InventorySlot _item) {
            for (int i = 0; i < Container.Items.Length; i++) {
                if (Container.Items[i].Item != _item.Item) continue;
                Container.Items[i].RemoveAmount(_item.Amount);
                return;
            }
        }

        protected override void OnBeforeSerialize() { }

        [SerializeField]
        private ScriptableObjectSaveInfo SaveInfo;

        [Button]
        public void save() {
            SaveInfo.ModifyInfo(this, "", "");
            SaveInfo.SaveScriptableObject();
        }

        [Button]
        public void load() {
            SaveInfo.LoadScriptableObject();
        }

    }

    [Serializable]
    public class Inventory {
        public InventorySlot[] Items = new InventorySlot[24];
    }

    [Serializable]
    public class InventorySlot {
        public Item Item;
        public int ID;
        public int Amount;

        public InventorySlot() {
            ID = -1;
            Item = null;
            Amount = 0;
        }

        public void UpdateSlot(int _id, Item _item, int _amount) {
            this.Item = _item;
            this.ID = _id;
            this.Amount = _amount;
        }

        public InventorySlot(int _id, Item _item, int _amount) {
            this.Item = _item;
            this.ID = _id;
            this.Amount = _amount;
        }

        public void AddAmount(int value) {
            Amount += value;
        }
        
        //Remove item amount, if it goes down below 0, set it to 0 and update the slot with default values
        public void RemoveAmount(int value) {
            Amount -= value;
            if (Amount > 0) return;
            ID = -1;
            Item = null;
            Amount = 0;
        }
    }
}