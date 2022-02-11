using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace RPGSystems {
    public abstract class UserInterface : MonoBehaviour {

        public InventoryObject inventoryObject;

        protected Dictionary<GameObject, InventorySlot> SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        // protected Color _originalSlotColor;
        // protected Color _modifiedSlotColor;
        // private Color _originalSpriteColor;

        public virtual void Init() {
            for (var i = 0; i < inventoryObject.Container.Items.Length; i++) {
                inventoryObject.Container.Items[i].parent = this;
            }
            CreateInventorySlots();
            
            AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnUIEnter(gameObject); });
            AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnUIExit(gameObject); });
            
        }

        private void Update() {
            // UpdateDisplay();
        }

        public abstract void CreateInventorySlots();

        public void UpdateDisplay() {
            foreach (var item in SlotsOnInterface) {
                if (item.Value.Item.ID < 0) {
                    item.Key.transform.GetComponentInChildren<TextMeshProUGUI>().text = "";
                    item.Key.transform.GetChild(0).GetComponent<Image>().sprite = null;
                    item.Key.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    continue;
                }
                item.Key.transform.GetComponentInChildren<TextMeshProUGUI>().text = item.Value.Amount == 1 ? "" : item.Value.Amount.ToString("n0");
                item.Key.transform.GetChild(0).GetComponent<Image>().sprite = inventoryObject.DatabaseObject.GetItem[item.Value.Item.ID].UIDisplay;
                item.Key.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }

        protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action) {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry {
                eventID = type,
                callback = new EventTrigger.TriggerEvent()
            };
            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }

        public void OnUIEnter(GameObject obj) {
            MouseData.HoveringUserInterface = obj.GetComponent<UserInterface>();
            RPGControls.UpdateInterface();
        }

        public void OnUIExit(GameObject obj) {
            MouseData.HoveringUserInterface = null;
            RPGControls.UpdateInterface();
        }

        public void OnEnter(GameObject obj) {
            MouseData.HoveringObject = obj;
            if (SlotsOnInterface.ContainsKey(obj)) {
                // MouseData.HoveringInventorySlot = SlotsOnInterface[obj];
            }
            else {
                // MouseData.HoveringInventorySlot = null;
            }
            RPGControls.UpdateInterface();
        }

        public void OnExit(GameObject obj) {
            MouseData.HoveringObject = null;
            RPGControls.UpdateInterface();
        }

        public void OnBeginDrag(GameObject obj) {
            var mouseObject = new GameObject();
            var rt = mouseObject.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(obj.GetComponent<RectTransform>().sizeDelta.x, obj.GetComponent<RectTransform>().sizeDelta.y);
            mouseObject.transform.SetParent(transform.parent);
            if (SlotsOnInterface[obj].Item.ID < 0) return;
            var image = mouseObject.AddComponent<Image>();
            image.raycastTarget = false;
            image.sprite = inventoryObject.DatabaseObject.GetItem[SlotsOnInterface[obj].Item.ID].UIDisplay;
            image.color = new Color(1, 1, 1, .5f);

            MouseData.PickedObject = mouseObject;
            RPGControls.UpdateInterface();
            // MouseData.PickedInventorySlot = SlotsOnInterface[obj];
            // Color col = _originalSlotColor;
            // col.a = .9f;
            // GetSlotImages().ForEach(t => t.color = col);

        }

        public void OnEndDrag(GameObject obj) {
            if (MouseData.HoveringUserInterface == null | MouseData.HoveringObject == null) {
                SlotsOnInterface[obj].RemoveItem();
                Destroy(MouseData.PickedObject);
                RPGControls.UpdateInterface();
                return;
            }
            if (MouseData.HoveringObject) {
                InventorySlot slot = MouseData.HoveringUserInterface.SlotsOnInterface[MouseData.HoveringObject];
                if (slot.IsAllowed(SlotsOnInterface[obj].GetItemBaseData()) && SlotsOnInterface[obj].IsAllowed(slot.GetItemBaseData())) {
                    if (slot.Item.ID <= -1) {
                        slot.UpdateSlot(SlotsOnInterface[obj].Item.ID, SlotsOnInterface[obj].Item, SlotsOnInterface[obj].Amount);
                        SlotsOnInterface[obj].RemoveItem();
                        Destroy(MouseData.PickedObject);
                        RPGControls.UpdateInterface();
                        return;
                    }
                    
                    if (SlotsOnInterface[obj].Item.ID == slot.Item.ID) {
                        if (SlotsOnInterface[obj].Item.Stackable) {
                            slot.UpdateSlot(SlotsOnInterface[obj].Item.ID, SlotsOnInterface[obj].Item, SlotsOnInterface[obj].Amount + slot.Amount);
                            SlotsOnInterface[obj].RemoveItem();
                            Destroy(MouseData.PickedObject);
                            RPGControls.UpdateInterface();
                            return;
                        }
                        InventorySlot temp = new InventorySlot();
                        temp.UpdateSlot(slot.Item.ID, slot.Item, slot.Amount);
                        slot.UpdateSlot(SlotsOnInterface[obj].Item.ID, SlotsOnInterface[obj].Item, SlotsOnInterface[obj].Amount);
                        SlotsOnInterface[obj].UpdateSlot(temp.Item.ID, temp.Item, temp.Amount);
                        Destroy(MouseData.PickedObject);
                        RPGControls.UpdateInterface();
                        return;
                    }

                    InventorySlot temp2 = new InventorySlot();
                    temp2.UpdateSlot(slot.Item.ID, slot.Item, slot.Amount);
                    slot.UpdateSlot(SlotsOnInterface[obj].Item.ID, SlotsOnInterface[obj].Item, SlotsOnInterface[obj].Amount);
                    SlotsOnInterface[obj].UpdateSlot(temp2.Item.ID, temp2.Item, temp2.Amount);
                    Destroy(MouseData.PickedObject);
                    RPGControls.UpdateInterface();
                    return;
                }
                Destroy(MouseData.PickedObject);
                Debug.Log("Did nothing");
                RPGControls.UpdateInterface();
                return;
            }

        }

        public void OnDrag(GameObject obj) {
            if (MouseData.PickedObject == null) return;
            MouseData.PickedObject.transform.position = Input.mousePosition;
            RPGControls.UpdateInterface();
        }



        //Get all the image components from all the slots in the DisplayInventory dictionary
        public List<Image> GetSlotImages() {
            List<Image> images = new List<Image>();
            foreach (var item in SlotsOnInterface) {
                images.Add(item.Key.GetComponent<Image>());
            }
            return images;
        }

        //Get all the child image components from all the slots in the DisplayInventory dictionary
        public List<Image> GetSlotChildImages() {
            List<Image> images = new List<Image>();
            foreach (var item in SlotsOnInterface) {
                images.Add(item.Key.transform.GetComponentInChildren<Image>());
            }
            return images;
        }

    }
}