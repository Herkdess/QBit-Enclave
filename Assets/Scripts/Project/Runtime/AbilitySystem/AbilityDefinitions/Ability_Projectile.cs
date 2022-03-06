using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems.Abilities  {
    
    public abstract class Ability_Projectile : Ability_Definition {
        public int ProjectileCount = 1;
        protected int _projectileCount => (ProjectileCount + Stats.ProjectileCountAddition) * Stats.ProjectileCountMulti;
        public float ProjectileSpeed = 10f;
        protected float _projectileSpeed => (ProjectileSpeed + Stats.ProjectileSpeedAddition) * Stats.ProjectileSpeedMulti;
        
        protected Projectile_Singular_Data _projectileData;

        public override void SetupAbility(MonoBehaviour user) {
            base.SetupAbility(user);
            Debug.Log("Hello from Projectile Ability");
            _projectileData = new Projectile_Singular_Data();
            _projectileData.CanDamage = true;
            _projectileData.DamageValue = (int)_abilityDamage;
            _projectileData.DamageInterval = .2f;
            _projectileData.CurrentDamageInterval = 0;
            // _projectileData.CollisionCount = 0;
            // _projectileData.MaxCollisionCount = 100;
            _projectileData.DamageRadius = _abilityDamageRange;
        }

        protected override void UseAbility() {
            base.UseAbility();
        }


    }
    
    
}