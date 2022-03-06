using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems.Abilities {
    public abstract class Ability_Definition : ScriptableObject {

        [Header("Ability Base Info")]
        public Ability_GlobalStats Stats;
        public string AbilityName;

        public bool AutoCast;
        [HideIf("@AutoCast")]
        public KeyCode AbilityKey = KeyCode.Mouse0;
        private bool keyPressed = false;
        public float AbilityDamage;
        protected float _abilityDamage => (AbilityDamage + Stats.AbilityDamageAddition) * Stats.AbilityDamageMulti;
        
        public float AbilityCooldown;
        protected float _abilityCooldown => (AbilityCooldown + Stats.CDRAdd) * Stats.CDRMulti;
        
        [HideInInspector] public float AbilityCooldownCountdown;
        public float AbilityDuration; 
        protected float _abilityDuration => (AbilityDuration + Stats.DurationAdd) * Stats.DurationMulti;
        [HideInInspector] public float CurrentAbilityDuration;

        public float AbilityOrbitRange;
        protected float _abilityOrbitRange => (AbilityOrbitRange + Stats.AbilityOrbitRangeAddition) * Stats.AbilityOrbitRangeMulti;

        public float AbilityDamageRange;
        protected float _abilityDamageRange => (AbilityDamageRange + Stats.AbilityOrbitRangeAddition) * Stats.AbilityOrbitRangeMulti;
        
        public float AbilityCost;

        
        [HideInInspector] public MonoBehaviour AbilityOwner;
        [HideInInspector] public bool IsOnCooldown;
        [HideInInspector] public bool IsActive;

        
        /// <summary>
        /// Sets up the ability, use this to set up the ability for the first time.
        /// </summary>
        /// <param name="user"></param>
        public virtual void SetupAbility(MonoBehaviour user) {
            this.AbilityOwner = user;
            AbilityCooldownCountdown = _abilityCooldown;
            CurrentAbilityDuration = _abilityDuration;
            IsOnCooldown = false;
        }

        /// <summary>
        /// Checks if the ability is ready to be used.
        /// If the ability is on cooldown, it will not use the ability.
        /// </summary>
        /// <param name="previous"></param>
        public virtual void UseAbility(bool previous) {
            if (IsOnCooldown) return;
            if (!(Stats.UserMana >= AbilityCost)) return;
            Stats.UserMana -= AbilityCost;
            CurrentAbilityDuration = _abilityDuration;
            IsOnCooldown = true;
            UseAbility();
        }
        
        /// <summary>
        /// Actually uses the ability, use it to set the ability to active and set effects of the ability.
        /// </summary>
        protected virtual void UseAbility() {
            IsActive = true;
            Debug.Log($"{AbilityName} is Used");
        }
        
        /// <summary>
        /// Controls the duration and cooldown of the ability.
        /// </summary>

        public virtual void AbilityLifeCycle() {
            if (IsOnCooldown) {
                AbilityCooldownCountdown -= Time.deltaTime;
                if (AbilityCooldownCountdown <= .2f && Input.GetKeyDown(AbilityKey)) {
                    keyPressed = true;
                }
                if (AbilityCooldownCountdown <= 0) {
                    IsOnCooldown = false;
                    AbilityCooldownCountdown = _abilityCooldown;
                }
            }
            
            if (AutoCast) {
                UseAbility(false);
            }
            if(!AutoCast && Input.GetKeyDown(AbilityKey)) {
                keyPressed = true;
                // Debug.Log("Ability key pressed");
            }
            if(keyPressed) {
                Debug.Log("Press ability used");
                keyPressed = false;
                UseAbility(false);
            }
            
            if (!IsActive) return;
            if (CurrentAbilityDuration > 0) {
                CurrentAbilityDuration -= Time.deltaTime;
                if (CurrentAbilityDuration <= 0) {
                    AbilityLifeCycleEnd();
                }
            }
            

        }

        /// <summary>
        /// Fires on the end of the ability's life cycle.
        /// </summary>
        protected virtual void AbilityLifeCycleEnd() {
            IsActive = false;
            // Debug.Log($"{AbilityName} is Ended");
        }
    }
}