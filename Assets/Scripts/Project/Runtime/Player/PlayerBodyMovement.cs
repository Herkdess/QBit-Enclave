using System;
using Base;
using DG.Tweening;
using Project.Runtime.Player;
using UnityEngine;
public class PlayerBodyMovement : MonoBehaviour {

    private bool Move = false;
    public float MovementSpeed = 10;
    public float RotationSpeed = 10;
    public float DashCooldown = .2f;
    public float DashSpeed = 800;

    private bool MouseDash = false;
    
    private bool Dashing;

    Tween dashTween;

    private void Update() {
        if (!Move) {
            return;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space)) {
            if (!Dashing) {
                Dash();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) {
            Dash();
        }

        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * MovementSpeed * Time.deltaTime;
        if (Dashing) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            MouseDash = !MouseDash;
        }

    }

    private void LateUpdate() {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += movement * MovementSpeed * Time.deltaTime;
    }
    
    public void Dash() {
        if(Dashing) return;
        Move = Dashing = true;
        dashTween?.Kill();
        
        Vector3 dir = transform.forward;

        if (MouseDash) {
            dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            dir.z = 0;
            dir.Normalize();
        }
        else {
            dir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        }
        
        dashTween = transform.DOMove(transform.position + dir * DashSpeed * Time.deltaTime, .25f).SetEase(Ease.InOutQuad).OnComplete(() => {
            DashCooldown.Timer().OnComplete(
                () => {
                    Dashing = false;
                }
            );
        });
    }
    
    public void MovePlayer(Vector3 direction) {
        Move = true;
        transform.position += direction * MovementSpeed * Time.deltaTime;
    }

    public void Init() {
        Move = true;
    }
    

}