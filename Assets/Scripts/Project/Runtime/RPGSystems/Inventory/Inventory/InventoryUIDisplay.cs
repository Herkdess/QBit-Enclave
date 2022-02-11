using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace RPGSystems {
    public class InventoryUIDisplay : MonoBehaviour {
        
        public MouseItem mouseItem = new MouseItem();
        
        public InventoryObject inventoryObject;
        public GameObject InventoryPrefab;
        public Vector2Int UIStartPosition;
        public Vector2Int Padding;
        public int NumberOfColumn;
        private Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

        private Color _originalSlotColor;
        private Color _modifiedSlotColor;
        private Color _originalSpriteColor;
        
        private void Start() {
            inventoryObject.DatabaseObject.load();
            CreateInventorySlots();
        }

        private void Update() {
            UpdateDisplay();
        }

        void CreateInventorySlots() {
            itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
            for (int i = 0; i < inventoryObject.Container.Items.Length; i++) {
                var obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                
                AddEvent(obj, EventTriggerType.PointerEnter, delegate {OnEnter(obj);});
                AddEvent(obj, EventTriggerType.PointerExit, delegate {OnExit(obj);});
                AddEvent(obj, EventTriggerType.BeginDrag, delegate {OnBeginDrag(obj);});
                AddEvent(obj, EventTriggerType.EndDrag, delegate {OnEndDrag(obj);});
                AddEvent(obj, EventTriggerType.Drag, delegate {OnDrag(obj);});
                
                itemsDisplayed.Add(obj, inventoryObject.Container.Items[i]);
                _originalSlotColor = obj.GetComponent<Image>().color;
            }
            _modifiedSlotColor = _originalSlotColor;
            _modifiedSlotColor.a = 0f;
        }
        
        public void UpdateDisplay() {
            foreach (var item in itemsDisplayed) {
                if (item.Value.ID < 0) {
                    item.Key.transform.GetComponentInChildren<TextMeshProUGUI>().text = "";
                    item.Key.transform.GetChild(0).GetComponent<Image>().sprite = null;
                    item.Key.transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,0);
                    continue;
                }
                item.Key.transform.GetComponentInChildren<TextMeshProUGUI>().text = item.Value.Amount == 1 ? "" : item.Value.Amount.ToString("n0");
                item.Key.transform.GetChild(0).GetComponent<Image>().sprite = inventoryObject.DatabaseObject.GetItem[item.Value.ID].UIDisplay;
                item.Key.transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
            }
        }

        private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action) {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry {
                eventID = type,
                callback = new EventTrigger.TriggerEvent()
            };
            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }

        public void OnEnter(GameObject obj) {
            mouseItem.hoverObj = obj;
            if(itemsDisplayed.ContainsKey(obj)) {
                mouseItem.hoverItem = itemsDisplayed[obj];
            }
            else {
                mouseItem.hoverItem = null;
            }
        }
        
        public void OnExit(GameObject obj) {
            mouseItem.hoverObj = null;
            mouseItem.hoverItem = null;
        }
        
        public void OnBeginDrag(GameObject obj) {
            var mouseObject = new GameObject();
            var rt = mouseObject.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(obj.GetComponent<RectTransform>().sizeDelta.x, obj.GetComponent<RectTransform>().sizeDelta.y);
            mouseObject.transform.SetParent(transform.parent);
            if(itemsDisplayed[obj].ID < 0) return;
            var image = mouseObject.AddComponent<Image>();
            image.raycastTarget = false;
            image.sprite = inventoryObject.DatabaseObject.GetItem[itemsDisplayed[obj].ID].UIDisplay;
            image.color = new Color(1,1,1,.5f);
            
            mouseItem.obj = mouseObject;
            mouseItem.item = itemsDisplayed[obj];
            Color col = _originalSlotColor;
            col.a = .9f;
            GetSlotImages().ForEach(t => t.color = col);
            
        }
        
        public void OnEndDrag(GameObject obj) {
            if (mouseItem.hoverObj) {
                inventoryObject.ReplaceItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
            }
            else {
                inventoryObject.RemoveItem(itemsDisplayed[obj]);
            }
            mouseItem.ResetMouseItem();
            GetSlotImages().ForEach(t => t.color = _originalSlotColor);
        }
        
        public void OnDrag(GameObject obj) {
            if(mouseItem.obj == null) return;
            mouseItem.obj.transform.position = Input.mousePosition;
        }

        public Vector3 GetPosition(int i) {
            return new Vector3(UIStartPosition.x +(Padding.x * (i % NumberOfColumn)), UIStartPosition.y +(-Padding.y * (i / NumberOfColumn)), 0f);
        }

        //Get all the image components from all the slots in the DisplayInventory dictionary
        public List<Image> GetSlotImages() {
            List<Image> images = new List<Image>();
            foreach (var item in itemsDisplayed) {
                images.Add(item.Key.GetComponent<Image>());
            }
            return images;
        }
        
        //Get all the child image components from all the slots in the DisplayInventory dictionary
        public List<Image> GetSlotChildImages() {
            List<Image> images = new List<Image>();
            foreach (var item in itemsDisplayed) {
                images.Add(item.Key.transform.GetComponentInChildren<Image>());
            }
            return images;
        }


    }

    public class MouseItem {
        public GameObject obj;
        public InventorySlot item;
        public GameObject hoverObj;
        public InventorySlot hoverItem;

        public void ResetMouseItem() {
            GameObject.Destroy(obj);
            obj = null;
            item = null;
            hoverObj = null;
            hoverItem = null;
        }
    }
}