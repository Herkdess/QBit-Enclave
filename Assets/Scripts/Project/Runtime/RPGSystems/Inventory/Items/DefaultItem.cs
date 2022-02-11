using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems {
    [CreateAssetMenu(fileName = "NewDefaultItem", menuName = "RPG/Inventory/Items/Default WorldItem")]
    public class DefaultItem : ItemObject {

        private void Awake() {
            type = ItemType.Default;
        }
    }
}