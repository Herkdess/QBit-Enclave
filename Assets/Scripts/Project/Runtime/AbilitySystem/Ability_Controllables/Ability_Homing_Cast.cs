using UnityEngine;
namespace RPGSystems.Abilities {
    public class Ability_Homing_Cast {
        [Header("Homing Cast")]
        [HideInInspector] public MonoBehaviour parent;
        
        
    }

    public struct Ability_Homing_Cast_Data {
        public Transform target;
        public float speed;
        public float range;
        public float damage;   
        
    }
}