using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems.Abilities  {
    [CreateAssetMenu(fileName = "Projectile Ability", menuName = "RPG/Ability/Circling Objects")]
    public class Ability_CirclingObjects : Ability_Projectile {
        [HideLabel]
        public Ability_Sphere_Cast Cast;
        private List<Ability_Sphere_Cast> _spawners = new List<Ability_Sphere_Cast>();
        private Ability_Sphere_Cast newSpawn;

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
            newSpawn = new Ability_Sphere_Cast(Cast);
            newSpawn.SpawnSphere(_abilityDuration);
            Debug.Log("Used Ability");
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
            Cast.RotateRadius = _abilityRange;
        }
    }
}