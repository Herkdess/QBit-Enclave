using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos.RPGEditor;
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
                RPGControls.UpdateInterface();
                return;
            }
            
            for (int i = 0; i < Container.Items.Length; i++) {
                if (Container.Items[i].Item.ID != _item.ID) continue;
                Container.Items[i].AddAmount(_amount);
                RPGControls.UpdateInterface();
                return;
            }
            RPGControls.UpdateInterface();
            SetFirstEmptySlot(_item, _amount);
        }

        
        public InventorySlot GetFirstEmptySlot() {
            return Container.Items.FirstOrDefault(t => t.Item.ID <= -1);
        }
        
        public InventorySlot SetFirstEmptySlot(Item _item, int _amount) {
            var slot = GetFirstEmptySlot();
            slot?.UpdateSlot(_item.ID, _item, _amount);
            //If the slot is null, setup a function to drop the inventory back on the ground
             
            return slot;
        }

        public void ReplaceItem(InventorySlot _item1, InventorySlot _item2) {
            InventorySlot temp = new InventorySlot(_item2.Item.ID, _item2.Item, _item2.Amount);
            _item2.UpdateSlot(_item1.Item.ID, _item1.Item, _item1.Amount);
            _item1.UpdateSlot(temp.Item.ID, temp.Item, temp.Amount);
        }

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

        [Button]
        void ResetInventory() {
            for (int i = 0; i < Container.Items.Length; i++) {
                Container.Clear();
            }
            save();
        }

    }

    [Serializable]
    public class Inventory {
        public InventorySlot[] Items = new InventorySlot[24];
        public void Clear() {
            for (int i = 0; i < Items.Length; i++) {
                Items[i].UpdateSlot(-1, new Item(), 0);
            }
        }
    }

    [Serializable]
    public class InventorySlot {
        public ItemType[] allowedTypes = new ItemType[0];
        public UserInterface parent;
        public Item Item;
        public int Amount;

        public InventorySlot() {
            Item = new Item();
            Amount = 0;
            parent = null;
        }

        public void UpdateSlot(int _id, Item _item, int _amount) {
            this.Item = _item;
            this.Item.ID = _id;
            this.Amount = _amount;
        }

        public InventorySlot(int _id, Item _item, int _amount) {
            this.Item = _item;
            this.Item.ID = _id;
            this.Amount = _amount;
        }

        public void AddAmount(int value) {
            Amount += value;
        }
        
        public void RemoveAmount(int value) {
            Amount -= value;
            if (Amount > 0) return;
            Item = null;
            Amount = 0;
        }

        public void RemoveItem() {
            Item = new Item();
            Amount = 0;
        }
        
        public bool IsAllowed(ItemObject _itemObject) {
            if(allowedTypes.Length == 0 || _itemObject == null) return true;
            for (int i = 0; i < allowedTypes.Length; i++) {
                if(_itemObject.type == allowedTypes[i]) return true;
            }
            return false;
        }
    }
}