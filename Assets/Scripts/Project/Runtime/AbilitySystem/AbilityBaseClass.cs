using System.Threading.Tasks;
using UnityEngine;
namespace RPGSystems {
    public abstract class AbilityBaseClass : ScriptableObject {

        [Header("Ability Base Info")]
        public GlobalAbilityStats AbilityStats;
        [HideInInspector] public MonoBehaviour AbilityOwner;
        public string AbilityName;
        public float StartCooldown;
        protected float _startCooldown => StartCooldown * AbilityStats.CDR;
        [HideInInspector] public float CurrentCooldown;
        public float AbilityDuration;
        protected float _abilityDuration => AbilityDuration * AbilityStats.Duration;
        public float AbilityCost;
        [HideInInspector] public float CurrentAbilityDuration;

        [HideInInspector] public bool IsOnCooldown;
        [HideInInspector] public bool IsActive;

        public virtual void SetupAbility(MonoBehaviour user) {
            // Debug.Log(_startCooldown);
            this.AbilityOwner = user;
            CurrentCooldown = _startCooldown;
            CurrentAbilityDuration = _abilityDuration;
            IsOnCooldown = false;
        }


        public virtual void UseAbility(bool previous) {
            if (IsOnCooldown) return;
            if (!(AbilityStats.UserMana >= AbilityCost)) return;
            AbilityStats.UserMana -= AbilityCost;
            CurrentAbilityDuration = _abilityDuration;
            IsOnCooldown = true;
            UseAbility();
        }

        public virtual void AbilityLifeCycle() {
            if (IsOnCooldown) {
                CurrentCooldown -= Time.deltaTime;
                if (CurrentCooldown <= 0) {
                    IsOnCooldown = false;
                    CurrentCooldown = _startCooldown;
                }
            }
            if (!IsActive) return;
            if (CurrentAbilityDuration > 0) {
                CurrentAbilityDuration -= Time.deltaTime;
                if (CurrentAbilityDuration <= 0) {
                    AbilityLifeCycleEnd();
                }
            }
        }

        protected virtual void UseAbility() {
            IsActive = true;
            Debug.Log($"{AbilityName} is Used");
        }

        protected virtual void AbilityLifeCycleEnd() {
            IsActive = false;
            Debug.Log($"{AbilityName} is Ended");
        }
    }
}