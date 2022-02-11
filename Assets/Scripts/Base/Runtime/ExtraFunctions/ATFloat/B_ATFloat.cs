using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = System.Object;
namespace Base {
    [Serializable]
    public class B_ATFloat {
        public float BaseValue;
        protected bool isDirty = true;
        protected float lastBaseValue = float.MinValue;
        protected float _value;
        public virtual float Value {
            get {
                if (isDirty || lastBaseValue != BaseValue) {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        protected readonly ReadOnlyCollection<B_ATModifier> StatModifiers;
        protected readonly List<B_ATModifier> statModifiers;

        public B_ATFloat() {
            statModifiers = new List<B_ATModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public B_ATFloat(float baseValue) : this() {
            BaseValue = baseValue;
        }

        public virtual void AddModifier(B_ATModifier mod) {
            mod.Parent = this;
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        public virtual void AddModifier(B_ATModifier mod, bool once) {
            if(statModifiers.Contains(mod)) return;
            mod.Parent = this;
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        public virtual void ModifyValue() {
            isDirty = true;
            statModifiers.Sort(CompareModifierOrder);
        }

        public virtual bool RemoveModifier(B_ATModifier mod) {
            if (statModifiers.Remove(mod)) {
                isDirty = true;
                mod.Parent = null;
                return true;
            }
            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source) {
            bool didRemove = false;

            for (int i = statModifiers.Count - 1; i >= 0; i--) {
                if (statModifiers[i].Source == source) {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

        public void RemoveAllModifiers() {
            // for (int i = statModifiers.Count - 1; i >= 0; i--) {
            //     isDirty = true;
            //     statModifiers.RemoveAt(i);
            // }
            statModifiers.Clear();
            isDirty = true;
        }

        protected virtual int CompareModifierOrder(B_ATModifier a, B_ATModifier b) {
            if (a.CalculationOrder < b.CalculationOrder)
                return -1;
            else if (a.CalculationOrder > b.CalculationOrder)
                return 1;
            return 0;
        }

        protected virtual float CalculateFinalValue() {
            float finalValue = BaseValue;
            float sumPercentToAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++) {

                B_ATModifier mod = statModifiers[i];

                if (mod.Type == AT_AttributeModifierType.Flat) {
                    finalValue += mod.Value;
                }
                else if (mod.Type == AT_AttributeModifierType.PercentMult) {
                    finalValue *= 1 + mod.Value;
                }
                else if (mod.Type == AT_AttributeModifierType.PercentAdd) {
                    sumPercentToAdd += mod.Value;
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != AT_AttributeModifierType.PercentAdd) {
                        finalValue *= 1 + sumPercentToAdd;
                        sumPercentToAdd = 0;
                    }
                }
            }
            return (float)Math.Round(finalValue, 4);
        }


    }

    public enum AT_AttributeModifierType {
        Flat = 100,
        PercentMult = 200,
        PercentAdd = 300

    }
    [Serializable]
    public class B_ATModifier {
        #region Odin
        private string info = "Set Type Value";
        private int orderMin = 0, orderMax = 100;
        #endregion
        [BoxGroup]
        [PropertyRange("orderMin", "orderMax")]
        public int CalculationOrder;
        [BoxGroup]
        [InfoBox("@info", InfoMessageType.None)]
        [SerializeField] private float _value = 100;
        public float Value {
            get => _value;
            set {
                _value = value;
                Parent?.ModifyValue();
            }
        }
        [BoxGroup]
        [OnValueChanged("AdjustOrder")]
        public AT_AttributeModifierType Type = AT_AttributeModifierType.Flat;
        public Object Source;
        [HideInInspector] public B_ATFloat Parent;
        public B_ATModifier(float value, AT_AttributeModifierType type, int calculationOrder, object source) {
            Value = value;
            Type = type;
            CalculationOrder = calculationOrder;
            Source = source;
        }

        void AdjustOrder() {
            CalculationOrder = (int)Type;
            orderMin = (int)Type;
            orderMax = (int)Type + 100;
            switch (Type) {
                case AT_AttributeModifierType.Flat:
                    info = "Adds the number to the original value";
                    break;
                case AT_AttributeModifierType.PercentMult:
                    info = "Multiplies value by its modified version, stacks with other modifiers";
                    break;
                case AT_AttributeModifierType.PercentAdd:
                    info = "Multiplies value by its modified version, doesn't stacks with other modifiers";
                    break;
                default:
                    info = "Set Type Value";
                    break;
            }
        }

        public B_ATModifier(float value, AT_AttributeModifierType type) : this(value, type, (int)type, null) { }
        public B_ATModifier(float value, AT_AttributeModifierType type, int calculationOrder) : this(value, type, calculationOrder, null) { }
        public B_ATModifier(float value, AT_AttributeModifierType type, object source) : this(value, type, (int)type, source) { }
    }
}