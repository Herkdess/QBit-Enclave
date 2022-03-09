using System;
using RPGSystems.Abilities;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Testing.ObjectCastShape {
    public class Caster : MonoBehaviour {
        public Ability_Projectile_Cast Cast;

        public Projectile_Singular_Data CastData;
        // private void Start() {
        //     Cast.Setup(this, CastData);
        //     Cast.Cast_Projectile();
        // }
        //
        // [Button]
        // public void Cast_Projectile() {
        //     Cast.ClearCast();
        //     Cast.Cast_Projectile();
        // }
        
        
    }
}