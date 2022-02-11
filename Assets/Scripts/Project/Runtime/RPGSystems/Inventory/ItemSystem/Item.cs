using System.Collections.Generic;
using RPGSystems;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems {
    [CreateAssetMenu(fileName = "NewItem", menuName = "RPG/Inventory/New Item")]
    public class Item : ScriptableObject {
        [HideLabel]
        public ItemObject item;

        [HideInInspector] public InventorySlot _inventorySlot;
        [HideInInspector] public bool Equipped;
        
        public void EquipItem(InventorySlot slot) {
            this._inventorySlot = slot;
            Equipped = true;
        }
        
        public void UnequipItem(InventorySlot slot) {
            this._inventorySlot = slot;
            Equipped = false;
        }

        public void AddItem(InventorySlot slot) {
            this._inventorySlot = slot;
            Equipped = false;
        }

        public void RemoveItem(InventorySlot slot) {
            this._inventorySlot = slot;
            Equipped = false;
        }
        
    }    
}