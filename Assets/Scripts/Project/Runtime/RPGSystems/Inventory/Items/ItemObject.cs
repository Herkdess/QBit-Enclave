using System;
using Base;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems {
    public enum ItemType {
        Food,
        Helmet,
        Armor,
        Weapon,
        Shield,
        boots,
        Default
    }
    public abstract class ItemObject : ScriptableObject {

        [PreviewField]
        public Sprite UIDisplay;
        public ItemType type;
        public string description;
        public bool Stackable;
        [HideLabel]
        public Item item;

    }


    [Serializable]
    public class Item {
        public int ID;
        public string ItemName;
        public bool Stackable;
        public Item() {
            ID = -1;
            ItemName = "";
        }
        public Item(ItemObject item) {
            ItemName = item.name;
            ID = item.item.ID;
            Stackable = item.Stackable;
        }
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