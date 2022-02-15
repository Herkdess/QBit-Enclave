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
        // B_CES_CentralEventSystem.BTN_OnStartPressed.AddFunction(SetupModules, false);
    }

    public void SetupModules() {
        bodyMovement = GetComponent<PlayerBodyMovement>();
        bodyMovement.Init();
        ActiveVirtualCameras.VirCam1.CameraSetAll(transform);
    }

    private void Update() {
        _previousPosition = transform.position;
        if (Input.GetButton("Fire2")) {
            if (Time.time > nextFire) {
                nextFire = Time.time + FireRate;
                CreateParticleEffect();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D col) {
        

    }
    public void CreateParticleEffect() {
        GameObject particleEffect = Instantiate(Resources.Load<GameObject>("Prefabs/ParticleEffect"), transform.position, Quaternion.identity);
        
        Vector3 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        mouseDirection.z = 0;
        mouseDirection.Normalize();
        particleEffect.GetComponent<TestFire>().Setup(mouseDirection, GetVelocity());
        // particleEffect.transform.DOMove(particleEffect.transform.position + mouseDirection * FireRange * Time.deltaTime, .5f).SetEase(Ease.InFlash).OnComplete(() => { Destroy(particleEffect); });
    }
    
    //Calculate the velocity of the transform with record of the last two frames
    Vector3 _previousPosition;
    private Vector3 CalculateVelocity() {
        return (transform.position - _previousPosition) / Time.deltaTime;
    }
    
    //Give the float value of the velocity
    public float GetVelocity() {
        return CalculateVelocity().magnitude;
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