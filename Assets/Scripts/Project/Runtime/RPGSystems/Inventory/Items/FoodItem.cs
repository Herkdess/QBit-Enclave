using System;
using UnityEngine;
namespace RPGSystems {
    [CreateAssetMenu(fileName = "NewFoodObject", menuName = "RPG/Inventory/Items/Food WorldItem")]
    public class FoodItem : ItemObject {
        public float RestoreHealthValue;
        private void Awake() {
            type = ItemType.Food;
        }
    }
}