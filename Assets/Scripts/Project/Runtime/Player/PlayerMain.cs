using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using Base.UI;
using DG.Tweening;
using Project.Runtime.Player;
using RPGSystems;
using UnityEngine;
public class PlayerMain : RPGSystemUser {
    
    PlayerBodyMovement bodyMovement;
    private float FireRange = 3000f;
    private float FireRate = 0.5f;
    private float nextFire = 0f;

    public override void Initialize() {
        base.Initialize();
    }

    private void Start() {
        B_CES_CentralEventSystem.BTN_OnStartPressed.AddFunction(SetupModules, false);
    }

    void SetupModules() {
        bodyMovement = GetComponent<PlayerBodyMovement>();
        bodyMovement.Init();
    }

    private void Update() {
        if (Input.GetButton("Fire2")) {
            if (Time.time > nextFire) {
                nextFire = Time.time + FireRate;
                CreateParticleEffect();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D col) {
        
        if (col.TryGetComponent(out WorldItem item)) {
            Storage.AddItem(new Item(item.item), 1);
            Destroy(col.gameObject);
        }
    }
    
    public void CreateParticleEffect() {
        GameObject particleEffect = Instantiate(Resources.Load<GameObject>("Prefabs/ParticleEffect"), transform.position, Quaternion.identity);
        //Initialize itemCollectorFire in particleEffect
        particleEffect.GetComponent<ItemCollectorFire>().Setup(Storage);
        // particleEffect.transform.DOMove(Camera.main.ScreenToWorldPoint(Input.mousePosition), .5f).OnComplete(() => { Destroy(particleEffect); });
        Vector3 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        mouseDirection.z = 0;
        mouseDirection.Normalize();
        particleEffect.transform.DOMove(particleEffect.transform.position + mouseDirection * FireRange * Time.deltaTime, .5f).SetEase(Ease.InFlash).OnComplete(() => { Destroy(particleEffect); });
    }

    protected override void OnDeath() {
        base.OnDeath();
    }

    public override void TakeDamage(float value) {
        base.TakeDamage(value);
    }

    protected override void Uninitialize() {
        base.Uninitialize();
    }
}