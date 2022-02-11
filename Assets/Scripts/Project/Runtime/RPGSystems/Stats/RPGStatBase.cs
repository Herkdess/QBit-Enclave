using System.Collections.Generic;
using Base;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems {
    [CreateAssetMenu(fileName = "RPGStatBase", menuName = "RPG/Stats/New Stat User")]
    public class RPGStatBase : SerializedScriptableObject {

        #region Attributes
        
        [FoldoutGroup("Attributes")]
        public RPGAttributes Health = new RPGAttributes(RPGAttributeTypes.Health, 100, false);
        [FoldoutGroup("Attributes")]
        public RPGAttributes Stamina = new RPGAttributes(RPGAttributeTypes.Stamina, 50, false);
        [FoldoutGroup("Attributes")]
        public RPGAttributes Mana = new RPGAttributes(RPGAttributeTypes.Mana, 50, false);
        [FoldoutGroup("Attributes")]
        public RPGAttributes Strength = new RPGAttributes(RPGAttributeTypes.Strength, 10, false);
        [FoldoutGroup("Attributes")]
        public RPGAttributes Dexterity = new RPGAttributes(RPGAttributeTypes.Dexterity, 10, false);
        [FoldoutGroup("Attributes")]
        public RPGAttributes WillPower = new RPGAttributes(RPGAttributeTypes.WillPower, 10, false);


        #endregion

        #region Stats

        [FoldoutGroup("Stats")]
        public RPGStats AttackPower = new RPGStats(RPGStatsTypes.AttackPower, 10, false);
        [FoldoutGroup("Stats")]
        public RPGStats AttackSpeed = new RPGStats(RPGStatsTypes.AttackSpeed, .5f, false);
        [FoldoutGroup("Stats")]
        public RPGStats CastPower = new RPGStats(RPGStatsTypes.CastPower, 10, false);
        [FoldoutGroup("Stats")]
        public RPGStats CastSpeed = new RPGStats(RPGStatsTypes.CastSpeed, 2, false);
        
        #endregion
        
        private Dictionary<RPGAttributeTypes, RPGAttributes> Attributes;

        private Dictionary<RPGStatsTypes, RPGStats> Stats;

        public void InitStats() {
            Attributes = new Dictionary<RPGAttributeTypes, RPGAttributes>();
            Stats = new Dictionary<RPGStatsTypes, RPGStats>();
            Attributes.Add(RPGAttributeTypes.Health, Health);
            Attributes.Add(RPGAttributeTypes.Stamina, Stamina);
            Attributes.Add(RPGAttributeTypes.Mana, Mana);
            Attributes.Add(RPGAttributeTypes.Strength, Strength);
            Attributes.Add(RPGAttributeTypes.Dexterity, Dexterity);
            Attributes.Add(RPGAttributeTypes.WillPower, WillPower);

            Stats.Add(RPGStatsTypes.AttackPower, AttackPower);
            Stats.Add(RPGStatsTypes.AttackSpeed, AttackSpeed);
            Stats.Add(RPGStatsTypes.CastPower, CastPower);
            Stats.Add(RPGStatsTypes.CastSpeed, CastSpeed);
        }

        public void EquipItem(Item itemObject) {
            AddItemAttributes(itemObject);
        }

        public bool CanEquipItem(ItemObject itemObject) {
            List<RPGAttributes> failedAttributesList = new List<RPGAttributes>();
            foreach (RPGAttributes req in itemObject.Requirements) {
                if (req.Value.Value > Attributes[req.attributeType].Value.Value) {
                    //If the required value is bigger than the current attributes, add it to the failed list
                    failedAttributesList.Add(req);
                }
            }
            if (failedAttributesList.Count > 0) {
                Debug.Log("Can't use item");
                return false;
            }
            return true;
        }

        void AddItemAttributes(Item itemObject) {
            foreach (var item in itemObject.item.ItemAttributeQualitiesList) {
                if (item.value.Value > 0) 
                    Attributes[item.type].AddBuff(item.value);
                else
                    Attributes[item.type].AddDebuff(item.value);
            }
            
            foreach (var item in itemObject.item.ItemStatQualitiesList) {
                if (item.value.Value > 0) 
                    Stats[item.type].AddBuff(item.value);
                else
                    Stats[item.type].AddDebuff(item.value);
            }
        }

        public void RemoveItem(Item itemObject) {
            foreach (var item in itemObject.item.ItemAttributeQualitiesList) {
                if (item.value.Value > 0) 
                    Attributes[item.type].RemoveBuff(item.value);
                else
                    Attributes[item.type].RemoveDebuff(item.value);
            }
            
            foreach (var item in itemObject.item.ItemStatQualitiesList) {
                if (item.value.Value > 0) 
                    Stats[item.type].RemoveBuff(item.value);
                else
                    Stats[item.type].RemoveDebuff(item.value);
            }
        }

        #region Save
        
        [FoldoutGroup("Save")]
        [HideLabel]
        [SerializeField]
        private ScriptableObjectSaveInfo SaveInfo;
        
        [FoldoutGroup("SaveOriginal")]
        public string OriginalSaveName;
        private string OriginalSavePath = "RPGSystem/Saves/Stats/Originals";
        
        
        [FoldoutGroup("Save")]
        [Button]
        public void SaveModifiedData() {
            SaveInfo.ModifyInfo(this, "", "");
            SaveInfo.SaveScriptableObject();
        }
        [FoldoutGroup("Save")]
        [Button]
        public void LoadModifiedData() {
            SaveInfo.LoadScriptableObject();
        }

        [FoldoutGroup("SaveOriginal")]
        [Button("SaveOriginalData")]
        public void SaveOriginalData() {
            ScriptableObjectSaveInfo info = new ScriptableObjectSaveInfo(this, $"{OriginalSavePath}", $"{OriginalSaveName}");
            info.SaveScriptableObject();
        }
        [FoldoutGroup("SaveOriginal")]
        [Button]
        public void LoadOriginalData() {
            ScriptableObjectSaveInfo info = new ScriptableObjectSaveInfo(this, $"{OriginalSavePath}", $"{OriginalSaveName}");
            info.LoadScriptableObject();
        }
        [FoldoutGroup("SaveOriginal")]
        [Button]
        public void ResetUser() { 
            Health = new RPGAttributes(RPGAttributeTypes.Health, 100, false);
            Stamina = new RPGAttributes(RPGAttributeTypes.Stamina, 100, false);
            Mana = new RPGAttributes(RPGAttributeTypes.Mana, 100, false);
            Strength = new RPGAttributes(RPGAttributeTypes.Strength, 100, false);
            WillPower = new RPGAttributes(RPGAttributeTypes.WillPower, 100, false);
                
            AttackPower = new RPGStats(RPGStatsTypes.AttackPower, 10, false);
            AttackSpeed = new RPGStats(RPGStatsTypes.AttackSpeed, 10, false);
            CastPower = new RPGStats(RPGStatsTypes.CastPower, 10, false);
            CastSpeed = new RPGStats(RPGStatsTypes.CastSpeed, 10, false);
            
            SaveOriginalData();
            SaveModifiedData();
        }
        
        #endregion

    }
}