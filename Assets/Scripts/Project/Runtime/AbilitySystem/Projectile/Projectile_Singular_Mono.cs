using System;
using UnityEngine;
namespace RPGSystems.Abilities {
    public class Projectile_Singular_Mono : MonoBehaviour {
        
        private Projectile_Singular_Data _data;
        
        public void Initialize(Projectile_Singular_Data data) {
            _data = new Projectile_Singular_Data(data);
        }

        public void Activate() {
            _data.CanDamage = true;
        }
        
        public void Deactivate() {
            _data.CanDamage = false;
        }
        
        private void Update() {
            if (_data.CanDamage) {
                _data.CurrentDamageInterval += Time.deltaTime;
                if (_data.CurrentDamageInterval >= _data.DamageInterval) {
                    _data.CurrentDamageInterval = 0;
                    Damage();
                }
            }
        }
        
        public void OnTriggerEnter(Collider other) {
            if (!_data.CanDamage) return;
            if (!other.gameObject.TryGetComponent(out HealthSystem healthSystem)) return;
            healthSystem.TakeDamage(_data.DamageValue);
            //
            // if (_data.CollisionCount < _data.MaxCollisionCount) {
            //     _data.CollisionCount++;
            // }
        }
        
        private void Damage() {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _data.DamageRadius);
            foreach (Collider collider in colliders) {
                if (collider.gameObject.TryGetComponent(out HealthSystem healthSystem)) {
                    healthSystem.TakeDamage(_data.DamageValue);
                }
            }
        }
    }
    
    public struct Projectile_Singular_Data {
        public bool CanDamage;
        public int DamageValue;
        public float DamageInterval;
        public float CurrentDamageInterval;
        public float DamageRadius;
        // public int MaxCollisionCount;
        // public int CollisionCount;

        public Projectile_Singular_Data(Projectile_Singular_Data data) {
            this.CanDamage = data.CanDamage;
            this.DamageValue = data.DamageValue;
            this.DamageInterval = data.DamageInterval;
            this.CurrentDamageInterval = data.CurrentDamageInterval;
            this.DamageRadius = data.DamageRadius;
            // this.MaxCollisionCount = data.MaxCollisionCount;
            // this.CollisionCount = data.CollisionCount;
        }

        public Projectile_Singular_Data(bool fresh) {
            this.CanDamage = false;
            this.DamageValue = 0;
            this.DamageInterval = 1f;
            this.CurrentDamageInterval = 0;
            this.DamageRadius = 1f;
            // this.MaxCollisionCount = 1;
            // this.CollisionCount = 0;
        }
    }
}