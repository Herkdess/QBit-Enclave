using System;
using RPGSystems;
using UnityEngine;
namespace Project.Runtime.Player {
    public class ItemCollectorFire : MonoBehaviour {
        InventoryObject inventory;
        private float movementSpeed = 8f;
        float lifeTime = .5f;
        public void Setup(InventoryObject inventory) {
            this.inventory = inventory;
            // lifeTime = .5f;
        }

        private void Update() {
            //count lifetime until it's over
            // lifeTime -= Time.deltaTime;
            // transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * movementSpeed);
            // if (lifeTime <= 0) {
            //     Destroy(gameObject);
            // }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.TryGetComponent(out WorldItem item)) {
                inventory.AddItem(new Item(item.item), 1);
                Destroy(col.gameObject);
            }
        }
    }
}