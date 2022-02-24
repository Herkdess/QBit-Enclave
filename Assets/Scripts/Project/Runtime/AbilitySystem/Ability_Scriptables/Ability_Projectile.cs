using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems.Abilities  {
    
    public abstract class Ability_Projectile : Ability_Base {
        public int ProjectileCount = 1;
        protected int _projectileCount => (ProjectileCount + Stats.ProjectileCountAddition) * Stats.ProjectileCountMulti;
        public float ProjectileSpeed = 10f;
        protected float _projectileSpeed => (ProjectileSpeed + Stats.ProjectileSpeedAddition) * Stats.ProjectileSpeedMulti;
        public float ProjectileLifeTime = 5f;
    }
    
    
}