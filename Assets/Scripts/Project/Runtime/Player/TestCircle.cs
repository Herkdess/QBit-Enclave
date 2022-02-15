using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class SphereSpawner {
    [Header("Sphere Spawner")]
    [HideInInspector] public MonoBehaviour parent;

    [Header("Rotation")]
    public float SpawnRadius = 1;
    public float RotateRadius = 10;
    public float SpawnHeight = 0;
    public float RotateHeight = 0;
    public float MaxRotateSpeed = 360;
    public float MinRotateSpeed = 0;

    private float _originalRotateRadius;

    public float RotateSpeed = 90;
    public Vector3 RotateAxis = new Vector3(0, 0, 1);
    Transform _rotateTransform;
    [Header("Spawn Control")]
    public GameObject SpawnObjectPrefab;
    public int SpawnCount = 10;
    public int MaxSpawnCount = 50;
    public int MinSpawnCount = 1;

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

    public void SpawnSphere(float duration) {
        this.Duration = duration;
        _rotateTransform = new GameObject("Rotate Transform").transform;
        _rotateTransform.SetParent(parent.transform);
        _rotateTransform.localPosition = Vector3.zero;
        _rotateTransform.localRotation = Quaternion.identity;
        _rotateTransform.localScale = Vector3.one;
        _spawnedObjects = new List<GameObject>();
        _originalRotateRadius = RotateRadius;
        RotateRadius = 1;
        for (int i = 0; i < SpawnCount; i++) {
            float angle = i * Mathf.PI * 2 / SpawnCount;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * SpawnRadius;
            GameObject go = GameObject.Instantiate(SpawnObjectPrefab, pos, Quaternion.identity);
            go.transform.parent = _rotateTransform.transform;
            _spawnedObjects.Add(go);
        }
        ChangeRotateRadius(_originalRotateRadius, 1f);
        Rotate();
        Move();
        StartCountdown();
    }

    public SphereSpawner(SphereSpawner previousSpawner) {
        this.parent = previousSpawner.parent;
        this.SpawnObjectPrefab = previousSpawner.SpawnObjectPrefab;
        this.SpawnCount = previousSpawner.SpawnCount;
        this.MaxSpawnCount = previousSpawner.MaxSpawnCount;
        this.MinSpawnCount = previousSpawner.MinSpawnCount;
        this.SpawnRadius = previousSpawner.SpawnRadius;
        this.RotateRadius = previousSpawner.RotateRadius;
        this.SpawnHeight = previousSpawner.SpawnHeight;
        this.RotateHeight = previousSpawner.RotateHeight;
        this.MaxRotateSpeed = previousSpawner.MaxRotateSpeed;
        this.MinRotateSpeed = previousSpawner.MinRotateSpeed;
        this.RotateSpeed = previousSpawner.RotateSpeed;
        this.RotateAxis = previousSpawner.RotateAxis;
    }

    public SphereSpawner() { }

    // public void IncreaseAmount(int by) {
    //     if (SpawnCount + by > MaxSpawnCount) {
    //         SpawnCount = MaxSpawnCount;
    //         return;
    //     }
    //     Clear();
    //     SpawnCount += by;
    //     SpawnSphere(Duration);
    // }
    //
    // public void DecreaseAmount(int by) {
    //     if (SpawnCount <= MinSpawnCount) return;
    //     if (SpawnCount - by <= MinSpawnCount) {
    //         SpawnCount = MinSpawnCount;
    //         _shrinkTween?.Kill();
    //         _shrinkTween = ChangeRotateRadius(0, 1f).OnComplete(Clear);
    //         return;
    //     }
    //     Clear();
    //     SpawnCount -= by;
    //     SpawnSphere(Duration);
    // }

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
            parent.StopCoroutine(_parentRotateRoutine);
        }
        _parentRotateRoutine = parent.StartCoroutine(RotateCoroutine());
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
            parent.StopCoroutine(_objectsRotateRoutine);
        }
        _objectsRotateRoutine = parent.StartCoroutine(MoveCoroutine());
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
            _countdownRoutine = parent.StartCoroutine(Countdown());
            return;
        }
        parent.StopCoroutine(_countdownRoutine);
        _countdownRoutine = parent.StartCoroutine(Countdown());

    }

    IEnumerator Countdown() {
        yield return new WaitForSeconds(Duration);
        Shrink();
    }

    public void StopParentRotation() {
        if (_parentRotateRoutine != null) {
            parent.StopCoroutine(_parentRotateRoutine);
        }
    }

    public void StopObjectsRotation() {
        if (_objectsRotateRoutine != null) {
            parent.StopCoroutine(_objectsRotateRoutine);
        }
    }

    public void DestroySphere() {
        Ended = true;
        StopParentRotation();
        StopObjectsRotation();
        Clear();
    }
}