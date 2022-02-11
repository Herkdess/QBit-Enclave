using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace RPGSystems {
    public class DynamicInterface : UserInterface {
        
        public GameObject InventoryPrefab;

        public Vector2Int UIStartPosition;
        public Vector2Int Padding;
        public int NumberOfColumn;

        public override void Init() {
            base.Init();
        }

        public override void CreateInventorySlots() {
            {
                SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();
                for (int i = 0; i < inventoryObject.Container.Items.Length; i++) {
                    var obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                    obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

                    AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
                    AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
                    AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnBeginDrag(obj); });
                    AddEvent(obj, EventTriggerType.EndDrag, delegate { OnEndDrag(obj); });
                    AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

                    SlotsOnInterface.Add(obj, inventoryObject.Container.Items[i]);
                }
            }
        }
        
        private Vector3 GetPosition(int i) {
            return new Vector3(UIStartPosition.x + (Padding.x * (i % NumberOfColumn)), UIStartPosition.y + (-Padding.y * (i / NumberOfColumn)), 0f);
        }
    }
}