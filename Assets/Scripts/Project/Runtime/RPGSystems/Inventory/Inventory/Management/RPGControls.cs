using UnityEngine;
namespace RPGSystems {
    public static class RPGControls {
        #region Resources Paths
        
        public const string DatabasesPath = "RPG/Databases/";
        public const string InventoriesPath = "RPG/Inventories/";
        public const string ItemPath = "RPG/Items/";
        public const string StatUsersPath = "RPG/StatUsers/";
        
        #endregion

        
        public static ItemDatabaseObject[] ItemDatabases;
        
        
        public static void Initialize() {
            ItemDatabases = Resources.LoadAll<ItemDatabaseObject>(DatabasesPath);
            foreach (var database in ItemDatabases) {
                database.Initialize();
            }
            
            foreach (var rpgSystemUser in GetAllStatUsers()) {
                rpgSystemUser.Initialize();
            }
            
            UpdateInterface();
            Debug.Log("All initialized");
        }
        
        public static void Uninitialize() {
            
        }

        #region Display

        public static void UpdateInterface() {
            foreach (var userInterface in GameObject.FindObjectsOfType<UserInterface>()) {
                userInterface.UpdateDisplay();
            }
        }

        public static ItemObject GetItemBaseData(this InventorySlot slot) {
            if(slot.Item == null || slot.Item.ID <= -1) {
                return null;
            }
            foreach (var database in ItemDatabases) {
                var item = database.items[slot.Item.ID];;
                if (item != null) {
                    return item;
                }
            }
            return null;
        }
        
        
        

        #endregion

        #region Helper Functions
        
        public static ItemDatabaseObject[] GetAllDatabases() {
            return Resources.LoadAll<ItemDatabaseObject>(DatabasesPath);
        }
        
        public static InventoryObject[] GetAllInventories() {
            return Resources.LoadAll<InventoryObject>(InventoriesPath);
        }

        public static ItemObject[] GetAllItems() {
            return Resources.LoadAll<ItemObject>(ItemPath);
        }

        public static RPGSystemUser[] GetAllStatUsers() {
            return Resources.LoadAll<RPGSystemUser>(StatUsersPath);
        }
        
        #endregion
        

        
    }
    
    public static class MouseData {
        public static UserInterface HoveringUserInterface;
        public static GameObject PickedObject;
        public static GameObject HoveringObject;

        public static void ResetMouseItem() {
            GameObject.Destroy(PickedObject);
            HoveringUserInterface= null;
            HoveringObject = null;
        }
    }
}