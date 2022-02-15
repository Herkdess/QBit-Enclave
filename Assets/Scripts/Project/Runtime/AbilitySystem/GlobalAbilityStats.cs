using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Global Ability Stats", menuName = "RPG/Global Ability Stats")]
public class GlobalAbilityStats : ScriptableObject {
    [Range(0.01f,3)]
    public float CDR = 1;
    [Range(.5f,10)]
    public float Duration = 1;
    [Range(50, 500)]
    public float UserMana = 100;

}
