using System.Collections.Generic;
using System.Linq;
using Base;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems {
    [CreateAssetMenu(fileName = "NewItemDatabase", menuName = "RPG/Inventory/WorldItem Database")]
    [DefaultExecutionOrder(-100)]
    public class ItemDatabaseObject : ScriptableObject {
        
        public ItemObject[] items;
        public Dictionary<int, ItemObject> GetItem;

        private int[] ids;
        private ItemObject[] itemObjects;

        public void Initialize() {
            load();
            GetBackups();
        }

        public void Uninitialize() {
            save();
        }
        [Button]
        void AutoSetup() {
            items = RPGControls.GetAllItems();
            SetIDs();
            save();
        }

        private void SetIDs() {
            GetItem = new Dictionary<int, ItemObject>();
            for (int i = 0; i < items.Length; i++) {
                items[i].item.ID = i;
                GetItem.Add(i, items[i]);
            }
        }

        [SerializeField]
        private ScriptableObjectSaveInfo SaveInfo;

        [Button]
        void save() {
            SetBackups();
            SaveInfo.ModifyInfo(this, "", "");
            SaveInfo.SaveScriptableObject();
        }

        [Button]
        void load() {
            SaveInfo.LoadScriptableObject();
        }

        void SetBackups() {
            ids = new int[GetItem.Count()];
            items = new ItemObject[GetItem.Count()];
            for (int i = 0; i < GetItem.Count(); i++) {
                ids[i] = GetItem.ElementAt(i).Key;
                items[i] = GetItem.ElementAt(i).Value;
            }
        }

        void GetBackups() {
            GetItem = new Dictionary<int, ItemObject>();
            for (int i = 0; i < ids.Length; i++) {
                GetItem.Add(ids[i], items[i]);
            }
        }
    }
}