using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using RPGSystems.Abilities;
using UnityEngine;

[System.Serializable]
public class Ability_Orbital_Cast : Ability_Projectile_Cast {


    [Header("Rotation")]
    public float RotateRadius = 10;
    public float RotateHeight = 0;
    public float MaxRotateSpeed = 360;
    public float MinRotateSpeed = 0;

    private float _originalRotateRadius;

    public float RotateSpeed = 90;
    public Vector3 RotateAxis = new Vector3(0, 0, 1);
    Transform _rotateTransform;


    private Tween _rotateTween;
    private Tween _heightTween;

    private Tween _shrinkTween;

    List<GameObject> _spawnedObjects = new List<GameObject>();

    Coroutine _parentRotateRoutine;
    Coroutine _objectsRotateRoutine;
    Coroutine _countdownRoutine;

    public float Duration;
    private float currentDuration;

    private bool Ended;

    public override void Setup(MonoBehaviour parent, Projectile_Singular_Data projectileData) {
        base.Setup(parent, projectileData);
    }
    

    public void SpawnOrbitter(float duration) {
        this.Duration = duration;
        _rotateTransform = new GameObject("Rotate Transform").transform;
        _rotateTransform.SetParent(CastData.parent.transform);
        _rotateTransform.localPosition = Vector3.zero;
        _rotateTransform.localRotation = Quaternion.identity;
        _rotateTransform.localScale = Vector3.one;
        _spawnedObjects = new List<GameObject>();
        _spawnedObjects = Cast_Projectile(_rotateTransform).ToList();
        
        _originalRotateRadius = RotateRadius;
        RotateRadius = 1;
        ChangeRotateRadius(_originalRotateRadius, 1f);
        Rotate();
        Move();
        StartCountdown();
    }

    public Ability_Orbital_Cast(Ability_Orbital_Cast previousCast) {
        this.CastData = new Projectile_Cast_Data(previousCast.CastData);
        this.RotateRadius = previousCast.RotateRadius;
        this.RotateHeight = previousCast.RotateHeight;
        this.MaxRotateSpeed = previousCast.MaxRotateSpeed;
        this.MinRotateSpeed = previousCast.MinRotateSpeed;
        this.RotateSpeed = previousCast.RotateSpeed;
        this.RotateAxis = previousCast.RotateAxis;
    }

    public Ability_Orbital_Cast() { }

    public void Shrink() {
        _originalRotateRadius = RotateRadius;
        ChangeRotateRadius(0, 1f).OnComplete(DestroySphere);
    }

    public void Shrink(float amount, float time) {
        _originalRotateRadius = RotateRadius;
        ChangeRotateRadius(amount, time).OnComplete(DestroySphere);
    }

    public void Clear() {
        GameObject.Destroy(_rotateTransform.gameObject);
        _spawnedObjects.Clear();
    }

    public Tween ChangeRotateRadius(float radius, float time) {
        _rotateTween?.Kill();
        _rotateTween = DOTween.To(() => RotateRadius, x => RotateRadius = x, radius, time).SetEase(Ease.InOutSine);
        return _rotateTween;
    }

    public Tween ChangeRotateHeight(float height, float time) {
        _heightTween?.Kill();
        _heightTween = DOTween.To(() => RotateHeight, x => RotateHeight = x, height, time).SetEase(Ease.InOutSine);
        return _heightTween;
    }


    public void Rotate() {
        if (_parentRotateRoutine != null) {
            CastData.parent.StopCoroutine(_parentRotateRoutine);
        }
        _parentRotateRoutine = CastData.parent.StartCoroutine(RotateCoroutine());
    }

    IEnumerator RotateCoroutine() {
        while (true) {
            if (Ended) {
                break;
            }
            _rotateTransform.Rotate(RotateAxis, RotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void Move() {
        if (_objectsRotateRoutine != null) {
            CastData.parent.StopCoroutine(_objectsRotateRoutine);
        }
        _objectsRotateRoutine = CastData.parent.StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine() {
        while (true) {
            if (Ended)
                break;
            for (int i = 0; i < _spawnedObjects.Count; i++) {
                if (Ended)
                    break;
                _spawnedObjects[i].transform.localPosition = new Vector3(Mathf.Sin(i * Mathf.PI * 2 / _spawnedObjects.Count), Mathf.Cos(i * Mathf.PI * 2 / _spawnedObjects.Count), 0) * RotateRadius + Vector3.up * RotateHeight;
            }
            yield return null;
        }
    }

    void StartCountdown() {
        if (_countdownRoutine == null) {
            _countdownRoutine = CastData.parent.StartCoroutine(Countdown());
            return;
        }
        CastData.parent.StopCoroutine(_countdownRoutine);
        _countdownRoutine = CastData.parent.StartCoroutine(Countdown());

    }

    IEnumerator Countdown() {
        yield return new WaitForSeconds(Duration);
        Shrink();
    }

    public void StopParentRotation() {
        if (_parentRotateRoutine != null) {
            CastData.parent.StopCoroutine(_parentRotateRoutine);
        }
    }

    public void StopObjectsRotation() {
        if (_objectsRotateRoutine != null) {
            CastData.parent.StopCoroutine(_objectsRotateRoutine);
        }
    }

    public void DestroySphere() {
        Ended = true;
        StopParentRotation();
        StopObjectsRotation();
        Clear();
    }
}