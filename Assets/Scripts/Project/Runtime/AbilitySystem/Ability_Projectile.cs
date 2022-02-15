using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems {
    [CreateAssetMenu(fileName = "Projectile Ability", menuName = "RPG/Ability/Projectile")]
    public class Ability_Projectile : AbilityBaseClass {
        
        [HideLabel]
        public SphereSpawner Spawner;
        private List<SphereSpawner> _spawners = new List<SphereSpawner>();
        private SphereSpawner newSpawn;

        public override void SetupAbility(MonoBehaviour user) {
            base.SetupAbility(user);
            Spawner.parent = user;
        }

        public override void UseAbility(bool previous) {
            base.UseAbility(previous);
        }

        public override void AbilityLifeCycle() {
            base.AbilityLifeCycle();
        }

        protected override void UseAbility() {
            base.UseAbility();
            newSpawn = new SphereSpawner(Spawner);
            newSpawn.SpawnSphere(_abilityDuration);
        }

        protected override void AbilityLifeCycleEnd() {
            base.AbilityLifeCycleEnd();
            // newSpawn.Shrink(0, 1f);
        }
    }
    
    
}