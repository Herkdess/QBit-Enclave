using UnityEngine;
namespace RPGSystems.Abilities {
    public class Ability_Projectile_Cast {
        [Header("Spawn Control")]
        public GameObject SpawnObjectPrefab;
        public int SpawnCount = 10;
        public int MaxSpawnCount = 50;
        public int MinSpawnCount = 1;

        public virtual void Cast_Projectile(float duration) {
            
        }
        
        
        
    }
}