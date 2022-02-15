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

    public List<AbilityBaseClass> Abilities;

    public override void Initialize() {
        base.Initialize();
    }

    private void Start() {
        // B_CES_CentralEventSystem.BTN_OnStartPressed.AddFunction(SetupModules, false);
        Abilities.ForEach(t => t.SetupAbility(this));
    }

    public void SetupModules() {
        bodyMovement = GetComponent<PlayerBodyMovement>();
        bodyMovement.Init();
        ActiveVirtualCameras.VirCam1.CameraSetAll(transform);
    }

    private void Update() {
        foreach (AbilityBaseClass ability in Abilities) {
            ability.AbilityLifeCycle();
            ability.UseAbility(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        

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