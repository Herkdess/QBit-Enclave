using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems {
    [CreateAssetMenu(fileName = "NewEquipmentItem", menuName = "RPG/Inventory/Items/Equipment WorldItem")]
    public class EquipmentObject : ItemObject {
        
        [TabGroup("Stats")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        public List<RPGAttributes> Requirements = new List<RPGAttributes>() {
            { new RPGAttributes(RPGAttributeTypes.Health, 0, false) },
            { new RPGAttributes(RPGAttributeTypes.Stamina, 0, false) },
            { new RPGAttributes(RPGAttributeTypes.Mana, 0, false) },
            { new RPGAttributes(RPGAttributeTypes.Strength, 0, false) },
            { new RPGAttributes(RPGAttributeTypes.Dexterity, 0, false) },
            { new RPGAttributes(RPGAttributeTypes.WillPower, 0, false) },
        };
        [TabGroup("Stats")]
        public List<ItemStatQualities> ItemStatQualitiesList;
        [TabGroup("Stats")]
        public List<ItemAttributeQualities> ItemAttributeQualitiesList;
        
        private void Awake() {
            type = ItemType.Armor;
        }
    }
}