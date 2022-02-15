using System;
using RPGSystems;
using UnityEngine;
namespace Project.Runtime.Player {
    public class TestFire : MonoBehaviour {

        Vector3 direction;
        private float movementSpeed = 8f;
        float lifeTime = .5f;
        public void Setup(Vector3 direction, float parentrelativespeed) {
            this.direction = direction;
            this.movementSpeed += parentrelativespeed;
        }


        private void Update() {
            transform.position += direction * movementSpeed * Time.deltaTime;
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0) {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D col) { }
    }
}