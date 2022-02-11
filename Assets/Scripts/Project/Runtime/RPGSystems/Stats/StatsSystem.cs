using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RPGSystems {
    public enum RPGStatsTypes {
        CastPower,
        CastSpeed,
        AttackPower,
        AttackSpeed,
    }

    public enum RPGAttributeTypes {
        Health,
        Stamina,
        Mana,
        Strength,
        Dexterity,
        WillPower
    }
    [Serializable]
    public class RPGStats {
        // [HideInInspector]
        private bool Item = true;
        [DisableIf("@Item == false")]
        public readonly RPGStatsTypes statsType;
        [HideLabel]
        public B_ATFloat Value;

        private List<object> buffs = new List<object>();
        private List<object> debuffs= new List<object>();

        public RPGStats(RPGStatsTypes type, float value, [CanBeNull] bool item = true) {
            this.statsType = type;
            this.Value = new B_ATFloat(value);
            this.Item = item;
        }

        public void ChangeBaseValue(float to) {
            Value.BaseValue = to;
        }

        public void AddToBaseValue(float add) {
            Value.BaseValue += add;
        }

        #region Buff Debuff Handling

        public void AddBuff(float value, [CanBeNull] AT_AttributeModifierType type = AT_AttributeModifierType.Flat, [CanBeNull] int order = 10) {
            object obj = new object();
            B_ATModifier mod = new B_ATModifier(value, type, order, obj);
            buffs.Add(obj);
            Value.AddModifier(mod);
        }

        public void AddBuff(B_ATModifier mod) {
            mod.Source ??= new object();
            buffs.Add(mod.Source);
            Value.AddModifier(mod);
        }

        public void RemoveBuff(B_ATModifier mod) {
            Value.RemoveModifier(mod);
            buffs.Remove(mod.Source);
        }

        public void RemoveBuff(object obj) {
            Value.RemoveAllModifiersFromSource(obj);
            buffs.Remove(obj);
        }

        public void RemoveAllBuffs() {
            if (buffs.IsNullOrEmpty()) {
                Debug.LogWarning("No Buffs Applied");
                return;
            }
            for (int i = buffs.Count - 1; i >= 0; i--) {
                RemoveBuff(buffs[i]);
            }
        }

        public void AddDebuff(in float value, [CanBeNull] AT_AttributeModifierType type = AT_AttributeModifierType.Flat, [CanBeNull] int order = 10) {
            object obj = new object();
            B_ATModifier mod = new B_ATModifier(value, type, order, obj);
            debuffs.Add(obj);
            Value.AddModifier(mod);
        }

        public void AddDebuff(in B_ATModifier mod) {
            mod.Source ??= new object();
            debuffs.Add(mod.Source);
            Value.AddModifier(mod);
        }

        public void RemoveDebuff(B_ATModifier mod) {
            Value.RemoveModifier(mod);
            debuffs.Remove(mod.Source);
        }

        public void RemoveDebuff(object obj) {
            Value.RemoveAllModifiersFromSource(obj);
            debuffs.Remove(obj);
        }

        public void RemoveAllDebuff() {
            if (buffs.IsNullOrEmpty()) {
                Debug.LogWarning("No Debuffs Applied");
                return;
            }
            for (int i = 0; i < debuffs.Count; i++) {
                RemoveDebuff(debuffs[i]);
            }
        }

        #endregion

    }

    [Serializable]
    public class RPGAttributes {

        private bool Item = true;
        [DisableIf("@Item == false")]
        public readonly RPGAttributeTypes attributeType;
        [HideLabel]
        public B_ATFloat Value;
        private List<object> buffs = new List<object>();
        private List<object> debuffs = new List<object>();
        
        public RPGAttributes(RPGAttributeTypes types, float value, [CanBeNull] bool item = true) {
            this.attributeType = types;
            this.Value = new B_ATFloat(value);
            this.Item = item;
            buffs = new List<object>();
            debuffs = new List<object>();
        }
        

        public void ChangeBaseValue(float to) {
            Value.BaseValue = to;
        }

        public void AddToBaseValue(float add) {
            Value.BaseValue += add;
        }

        #region Buff Debuff Handling

        public void AddBuff(float value, [CanBeNull] AT_AttributeModifierType type = AT_AttributeModifierType.Flat, [CanBeNull] int order = 10) {
            object obj = new object();
            B_ATModifier mod = new B_ATModifier(value, type, order, obj);
            buffs.Add(obj);
            Value.AddModifier(mod);
        }

        public void AddBuff(B_ATModifier mod) {
            mod.Source ??= new object();
            buffs.Add(mod.Source);
            Value.AddModifier(mod);
        }

        public void RemoveBuff(B_ATModifier mod) {
            Value.RemoveModifier(mod);
            buffs.Remove(mod.Source);
        }

        public void RemoveBuff(object obj) {
            Value.RemoveAllModifiersFromSource(obj);
            buffs.Remove(obj);
        }

        public void RemoveAllBuffs() {
            if (buffs.IsNullOrEmpty()) {
                Debug.LogWarning("No Buffs Applied");
                return;
            }
            for (int i = buffs.Count - 1; i >= 0; i--) {
                RemoveBuff(buffs[i]);
            }
        }

        public void AddDebuff(in float value, [CanBeNull] AT_AttributeModifierType type = AT_AttributeModifierType.Flat, [CanBeNull] int order = 10) {
            object obj = new object();
            B_ATModifier mod = new B_ATModifier(value, type, order, obj);
            debuffs.Add(obj);
            Value.AddModifier(mod);
        }

        public void AddDebuff(in B_ATModifier mod) {
            mod.Source ??= new object();
            debuffs.Add(mod.Source);
            Value.AddModifier(mod);
        }

        public void RemoveDebuff(B_ATModifier mod) {
            Value.RemoveModifier(mod);
            debuffs.Remove(mod.Source);
        }

        public void RemoveDebuff(object obj) {
            Value.RemoveAllModifiersFromSource(obj);
            debuffs.Remove(obj);
        }

        public void RemoveAllDebuff() {
            if (buffs.IsNullOrEmpty()) {
                Debug.LogWarning("No Debuffs Applied");
                return;
            }
            for (int i = 0; i < debuffs.Count; i++) {
                RemoveDebuff(debuffs[i]);
            }
        }

        #endregion
    }
}