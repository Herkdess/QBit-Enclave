using Base;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems {
    [CreateAssetMenu(fileName = "NewInventory", menuName = "RPG/Inventory/New Inventory")]
    public class Inventory : SerializedScriptableObject {
        [HideLabel]
        public InventoryObject inventoryObject;
        #region Save
        
        [SerializeField]
        private ScriptableObjectSaveInfo SaveInfo;

        public void InitInventory() {
            load();
            inventoryObject.InitInventory();
        }

        public void FlushInventory() {
            inventoryObject.FlushInventory();
            save();
        }
        
        [Button]
        public void save() {
            SaveInfo.ModifyInfo(this, "", "");
            SaveInfo.SaveScriptableObject();
        }

        [Button]
        void load() {
            SaveInfo.LoadScriptableObject();
        }

        [Button()]
        void ResetInventory() {
            InventoryType type = inventoryObject.type;
            inventoryObject = new InventoryObject();
            inventoryObject.type = type;
            inventoryObject.ChangeType();
            // inventoryObject.InitInventory(Parent);
        }

        #endregion


    }
}