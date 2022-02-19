using Base;
using UnityEngine;
public static class RPGExtentions {
    public const string RPG_PATH = "RPG/";
    public const string RPG_PATH_PREFAB = RPG_PATH + "Prefabs/";
    
    public static string RPG_SAVE_PATH => "RPG/Save/";
    public static string RPG_ITEM_SAVE_PATH = RPG_SAVE_PATH + "Items/";
    public static string RPG_CHARACTER_SAVE_PATH = RPG_SAVE_PATH + "Characters/";
    public static string RPG_ABILITY_PATH = RPG_SAVE_PATH + "PassiveAbilities/";
    
    
    public static void AdjustValues(this B_ATModifier atModifier) {
        atModifier.CalculationOrder = (int)atModifier.Type;
    }
}