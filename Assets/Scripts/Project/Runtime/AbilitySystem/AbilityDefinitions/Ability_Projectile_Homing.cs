using UnityEngine;
namespace RPGSystems.Abilities  {
    [CreateAssetMenu(fileName = "Projectile Ability", menuName = "RPG/Ability/Homing Projectile")]
    public class Ability_Projectile_Homing : Ability_Projectile {
        
        public Ability_Homing_Cast HomingCast;

        public override void SetupAbility(MonoBehaviour user) {
            base.SetupAbility(user);
        }

        public override void UseAbility(bool previous) {
            base.UseAbility(previous);
        }
        
        protected override void UseAbility() {
            base.UseAbility();
        }

        public override void AbilityLifeCycle() {
            base.AbilityLifeCycle();
        }

        protected override void AbilityLifeCycleEnd() {
            base.AbilityLifeCycleEnd();
        }

    }
}