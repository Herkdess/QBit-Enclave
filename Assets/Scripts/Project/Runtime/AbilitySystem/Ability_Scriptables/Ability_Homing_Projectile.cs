using UnityEngine;
namespace RPGSystems.Abilities  {
    [CreateAssetMenu(fileName = "Projectile Ability", menuName = "RPG/Ability/Homing Projectile")]
    public class Ability_Homing_Projectile : Ability_Projectile {

        // public Ability_Fire_Projectile projectile;
        public KeyCode AbilityKey = KeyCode.E;
        
        public override void SetupAbility(MonoBehaviour user) {
            base.SetupAbility(user);
        }

        public override void UseAbility(bool previous) {
            if(!Input.GetKeyDown(AbilityKey)) return;
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