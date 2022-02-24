using System.Collections;
using System.Collections.Generic;
using Base;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems {
    
        [CreateAssetMenu(fileName = "New Global Ability Stats", menuName = "RPG/Global Ability Stats")]
    public class Ability_GlobalStats : ScriptableObject {
        #region String Management

        private const string playerAttributesTabName = "Player Attributes";
        private const string playerAbilityAttributesTabName = "Ability Attributes";
        private const string abilitiesType = "Basic";
        private const string abilitiesBoxName = "Box";

        private const string originalSaveLocation = "Player_Attributes_Original";
        private const string currentSaveLocation = "Player_Attributes_Current";


        #endregion

        #region Ability Management


        [TabGroup("$playerAttributesTabName")]
        [Range(50, 500)]
        public float UserMana = 100;
        [TabGroup("$playerAttributesTabName")]
        [Range(50, 500)]
        public float UserHealth = 100;
        [TabGroup("$playerAttributesTabName")]
        [Range(5, 30)]
        public float UserMovementSpeed = 12;
        [FoldoutGroup("$playerAttributesTabName")]
        [HorizontalGroup("$playerAttributesTabName/$abilitiesType")]
        [BoxGroup("$playerAttributesTabName/$abilitiesType/$abilitiesBoxName", false)]
        [Header("CDR")]
        [Range(0.01f, 3)]
        public float CDRMulti = 1;
        [BoxGroup("$playerAttributesTabName/$abilitiesType/$abilitiesBoxName", false)]
        [Range(-50f, 50f)]
        public float CDRAdd = 0;
        [BoxGroup("$playerAttributesTabName/$abilitiesType/$abilitiesBoxName", false)]
        [Header("Duration")]
        [Range(.5f, 10)]
        public float DurationMulti = 1;
        [BoxGroup("$playerAttributesTabName/$abilitiesType/$abilitiesBoxName", false)]
        [Range(0, 50f)]
        public float DurationAdd = 0;
        [BoxGroup("$playerAttributesTabName/$abilitiesType/$abilitiesBoxName", false)]
        [Header("Projectile Count")]
        [Range(0, 5)]
        public int ProjectileCountMulti = 1;
        [BoxGroup("$playerAttributesTabName/$abilitiesType/$abilitiesBoxName", false)]
        [Range(0, 100)]
        public int ProjectileCountAddition = 0;
        [BoxGroup("$playerAttributesTabName/$abilitiesType/$abilitiesBoxName", false)]
        [Header("Projectile Speed")]
        [Range(0, 5)]
        public float ProjectileSpeedMulti = 1;
        [BoxGroup("$playerAttributesTabName/$abilitiesType/$abilitiesBoxName", false)]
        [Range(0, 100)]
        public float ProjectileSpeedAddition = 0;
        [BoxGroup("$playerAttributesTabName/$abilitiesType/$abilitiesBoxName", false)]
        [Header("Projectile Damage")]
        [Range(0, 5)]
        public float AbilityDamageMulti = 1;
        [BoxGroup("$playerAttributesTabName/$abilitiesType/$abilitiesBoxName", false)]
        [Range(0, 100)]
        public float AbilityDamageAddition = 0;
        [BoxGroup("$playerAttributesTabName/$abilitiesType/$abilitiesBoxName", false)]
        [Header("Projectile Range")]
        [Range(0, 5)]
        public float AbilityRangeMulti = 1;
        [BoxGroup("$playerAttributesTabName/$abilitiesType/$abilitiesBoxName", false)]
        [Range(0, 100)]
        public float AbilityRangeAddition = 0;

        #endregion

        #region Save Management

        [TabGroup("Save Management")]
        private ScriptableObjectSaveInfo OriginalInfo;
        [TabGroup("Save Management")]
        private ScriptableObjectSaveInfo CurrentInfo;

        [TabGroup("Save Management")]
        [Button]
        void SaveOriginal() {
            OriginalInfo.ModifyInfo(this, RPGExtentions.RPG_ABILITY_PATH, originalSaveLocation);
            OriginalInfo.SaveScriptableObject();
        }
        [TabGroup("Save Management")]
        [Button]
        void LoadOriginal() {
            OriginalInfo.LoadScriptableObject();
        }

        [TabGroup("Save Management")]
        [Button]
        void SaveCurrent() {
            CurrentInfo.ModifyInfo(this, RPGExtentions.RPG_ABILITY_PATH, currentSaveLocation);
            CurrentInfo.SaveScriptableObject();
        }

        [TabGroup("Save Management")]
        [Button]
        void LoadCurrent() {
            CurrentInfo.LoadScriptableObject();
        }

        #endregion


    }
}
