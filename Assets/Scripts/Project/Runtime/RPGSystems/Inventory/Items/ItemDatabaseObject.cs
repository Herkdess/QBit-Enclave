using System.Collections.Generic;
using Base;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems {
    [CreateAssetMenu(fileName = "NewItemDatabase", menuName = "RPG/Inventory/WorldItem Database")]
    [DefaultExecutionOrder(-100)]
    public class ItemDatabaseObject : ScriptableObject {
        
        public ItemObject[] items;
        public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

        [Button]
        public void SetIDs() {
            GetItem = new Dictionary<int, ItemObject>();
            for (int i = 0; i < items.Length; i++) {
                items[i].ID = i;
                GetItem.Add(i, items[i]);
            }
        }

        [SerializeField]
        private ScriptableObjectSaveInfo SaveInfo;

        [Button]
        public void save() {
            SaveInfo.ModifyInfo(this, "", "");
            SetIDs();
            SaveInfo.SaveScriptableObject();
        }

        [Button]
        public void load() {
            SaveInfo.LoadScriptableObject();
            SetIDs();
        }   
    }
}