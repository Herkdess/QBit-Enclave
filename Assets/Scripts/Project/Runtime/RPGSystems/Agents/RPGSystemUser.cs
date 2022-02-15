using Base;
using UnityEngine;
namespace RPGSystems {
    public abstract class RPGSystemUser : MonoBehaviour {

        public RPGStatBase Stats;
        public B_ATModifier DamageTaken;

        public virtual void Initialize() {
            Stats.InitStats();
            Stats.LoadModifiedData();
            DamageTaken = new B_ATModifier(0, AT_AttributeModifierType.Flat);
            Stats.Health.Value.AddModifier(DamageTaken);
        }

        protected virtual void Uninitialize() {
            Stats.Health.Value.RemoveModifier(DamageTaken);
            

            Stats.SaveModifiedData();
            Stats.LoadOriginalData();
        }

        public virtual void TakeDamage(float value) {
            if (Stats.Health.Value.Value <= 0) return;
            DamageTaken.Value -= value;
            if (Stats.Health.Value.Value <= 0) {
                OnDeath();
                return;
            }
            Debug.Log(Stats.Health.Value.Value);
        }


        protected virtual void OnDeath() {
            Debug.Log("Died");
        }


    }
}