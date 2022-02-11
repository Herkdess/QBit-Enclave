using System;
using System.Collections.Generic;
using Base;
using Microsoft.Unity.VisualStudio.Editor;
using RPGSystems;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems {

    [Serializable]
    public class ItemObject {

        [FoldoutGroup("Prime")]
        [PreviewField(80), HideLabel]
        [HorizontalGroup("Prime/Split", 80)]
        public Sprite ItemIcon;
        [VerticalGroup("Prime/Split/Right"), LabelWidth(120)]
        public string ItemName;
        [VerticalGroup("Prime/Split/Right"), LabelWidth(120)]
        [MultiLineProperty(3)]
        public string Description;
        [VerticalGroup("Prime/Split/Right"), LabelWidth(120)]
        public bool Stackable;
        [VerticalGroup("Prime/Split/Right"), LabelWidth(120)]
        public SlotType slotType = SlotType.Head;
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


    }
    
    [Serializable]
    public class ItemStatQualities {
        public RPGStatsTypes type;
        [HideLabel]
        public B_ATModifier value;

        public ItemStatQualities() {
            type = RPGStatsTypes.AttackPower;
            value = new B_ATModifier(0, AT_AttributeModifierType.Flat, this);
        }
    }

    [Serializable]
    public class ItemAttributeQualities {
        public RPGAttributeTypes type;
        public B_ATModifier value;

        public ItemAttributeQualities() {
            type = RPGAttributeTypes.Health;
            value = new B_ATModifier(0, AT_AttributeModifierType.Flat, this);
        }
    }

}