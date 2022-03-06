using System;
using UnityEngine;
namespace RPGSystems {
    public class HealthSystem : MonoBehaviour, Damageable {
        public int MaxHealth { get; set; }
        int _currentHealth { get; set; }
        
        public int CurrentHealth {
            get => _currentHealth;
            set {
                _currentHealth = value;
                if (_currentHealth <= 0) {
                    _currentHealth = 0;
                    if(IsDead) return;
                    OnDeath();
                }
            }
        }
        
        public bool IsDead => CurrentHealth <= 0;
        public bool CanBeDamaged { get; set; }
        
        public int DamageReductionFlat { get; set; }
        public int DamageReductionPercent { get; set; }

        public virtual void Setup(int maxHealth, int damageReductionFlat, int damageReductionPercent) {
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            DamageReductionFlat = damageReductionFlat;
            DamageReductionPercent = damageReductionPercent;
        }
        
        
        public virtual void TakeDamage(int damage) {
            if(!CanBeDamaged || IsDead) return;
            damage = Mathf.Max(damage - DamageReductionFlat, 0);
            damage = Mathf.RoundToInt(damage * (1 - (DamageReductionPercent / 100f)));
            CurrentHealth -= damage;
            if(CurrentHealth <= 0) {
                OnDeath();
            }
        }
        
        public void Heal(int heal) {
            if(IsDead) return;
            CurrentHealth = Mathf.Min(CurrentHealth + heal, MaxHealth);
        }

        public virtual void OnDeath() {
            if(IsDead) return;
            CurrentHealth = 0;
            CanBeDamaged = false;
        }
        
    }
    
    public interface Damageable {
        void TakeDamage(int damage);
    }
}