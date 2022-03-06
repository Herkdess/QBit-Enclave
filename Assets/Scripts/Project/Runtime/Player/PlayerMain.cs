using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
using Base.UI;
using DG.Tweening;
using RPGSystems;
using RPGSystems.Abilities;
using UnityEngine;
public class PlayerMain : RPGSystemUser {
    
    PlayerBodyMovement bodyMovement;

    public List<Ability_Definition> PassiveAbilities;
    public List<Ability_Definition> ActiveAbilities;
    public override void Initialize() {
        base.Initialize();
    }

    private void Start() {
        PassiveAbilities.ForEach(t => t.SetupAbility(this));
        ActiveAbilities.ForEach(t => t.SetupAbility(this));
    }

    public void SetupModules() {
        bodyMovement = GetComponent<PlayerBodyMovement>();
        bodyMovement.Init();
        ActiveVirtualCameras.VirCam1.CameraSetAll(transform);
    }

    private void Update() {
        foreach (Ability_Definition ability in PassiveAbilities) {
            ability.AbilityLifeCycle();
            // ability.UseAbility(true);
        }
        foreach (Ability_Definition ability in ActiveAbilities) {
            ability.AbilityLifeCycle();
            // ability.UseAbility(false);
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