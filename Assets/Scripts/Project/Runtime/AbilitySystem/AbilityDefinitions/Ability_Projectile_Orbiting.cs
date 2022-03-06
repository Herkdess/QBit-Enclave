using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems.Abilities  {
    [CreateAssetMenu(fileName = "Projectile Ability", menuName = "RPG/Ability/Projectile_Orbiting")] 
    public class Ability_Projectile_Orbiting : Ability_Projectile {
        [HideLabel]
        public Ability_Orbital_Cast Cast;
        private List<Ability_Orbital_Cast> _spawners = new List<Ability_Orbital_Cast>();
        private Ability_Orbital_Cast newSpawn;

        public override void SetupAbility(MonoBehaviour user) {
            base.SetupAbility(user);
            Cast.parent = user;
        }

        public override void UseAbility(bool previous) {
            base.UseAbility(previous);
        }
        
        protected override void UseAbility() {
            ModifyOriginalSpawner();
            base.UseAbility();
            newSpawn = new Ability_Orbital_Cast(Cast);
            newSpawn.SpawnOrbitter(_abilityDuration);
        }

        public override void AbilityLifeCycle() {
            base.AbilityLifeCycle();
        }

        protected override void AbilityLifeCycleEnd() {
            base.AbilityLifeCycleEnd();
        }

        void ModifyOriginalSpawner() {
            Cast.Duration = _abilityDuration;
            Cast.SpawnCount = _projectileCount;
            Cast.RotateSpeed = _projectileSpeed;
            Cast.RotateRadius = _abilityOrbitRange;
        }
    }
}