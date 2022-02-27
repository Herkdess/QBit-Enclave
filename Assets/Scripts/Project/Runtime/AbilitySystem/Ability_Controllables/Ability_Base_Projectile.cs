using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System;
using System.Collections;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace RPGSystems.Abilities {
    public abstract class Ability_Base_Projectile {

        protected MonoBehaviour _parent;

        public bool CanRotate = false;
        [HideIf("@CanRotate == false")]
        [Header("Rotation")]
        [HideLabel]
        public RotateData RotateData;

        [Header("Spawn Control")]
        public GameObject SpawnObjectPrefab;
        public int SpawnCount = 10;
        public int MaxSpawnCount = 50;
        public int MinSpawnCount = 1;

        private Tween _rotateTween;
        private Tween _heightTween;

        private Tween _shrinkTween;

        List<GameObject> _spawnedObjects = new List<GameObject>();


        Coroutine _countdownRoutine;

        public float Duration;
        private float currentDuration;

        private bool Ended;

        public void Initialize(MonoBehaviour parent) {
            _parent = parent;
        }

        public Ability_Base_Projectile() { }

        public Ability_Base_Projectile(Ability_Base_Projectile previousProjectile) {
            this._parent = previousProjectile._parent;
            this.SpawnObjectPrefab = previousProjectile.SpawnObjectPrefab;
            this.SpawnCount = previousProjectile.SpawnCount;
            this.MaxSpawnCount = previousProjectile.MaxSpawnCount;
            this.MinSpawnCount = previousProjectile.MinSpawnCount;
            // this.Rotate = previousProjectile.Rotate;
            this.RotateData = previousProjectile.RotateData;
            this.Duration = previousProjectile.Duration;
        }

        public void SpawnSphere(float duration) {
            this.Duration = duration;
            RotateData.GenerateRotateTransform(_parent);
            _spawnedObjects = new List<GameObject>();
            RotateData.RotateRadius = 1;
            for (int i = 0; i < SpawnCount; i++) {
                SpawnObject(i);
            }
            ChangeRotateRadius(RotateData._originalRotateRadius, 1f);
            Rotate();
            Move();
            StartCountdown();
        }

        GameObject SpawnObject(int index) {
            float angle = index * Mathf.PI * 2 / SpawnCount;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * RotateData.RotateRadius;
            GameObject go = GameObject.Instantiate(SpawnObjectPrefab, pos, Quaternion.identity);
            go.transform.parent = RotateData.RotateTransform;
            _spawnedObjects.Add(go);
            return go;
        }

        void Rotate() {
            if(RotateData._parentRotateRoutine != null) {
                _parent.StopCoroutine(RotateData._parentRotateRoutine);
            }
            RotateData._parentRotateRoutine = _parent.StartCoroutine(RotateCoroutine());
        }

        void Move() {
            if(RotateData._objectsRotateRoutine != null) {
                _parent.StopCoroutine(RotateData._objectsRotateRoutine);
            }
            RotateData._objectsRotateRoutine = _parent.StartCoroutine(MoveCoroutine());
        }

        void Shrink() {
            RotateData._originalRotateRadius = RotateData.RotateRadius;
            ChangeRotateRadius(0, 1).OnComplete(DestroySphere);
        }

        void Shrink(float amount, float time) {
            RotateData._originalRotateRadius = RotateData.RotateRadius;
            ChangeRotateRadius(amount, time).OnComplete(DestroySphere);
        }
        

        Tween ChangeRotateRadius(float radius, float time) {
            _rotateTween?.Kill();
            _rotateTween = DOTween.To((x) => RotateData.RotateRadius = x, RotateData.RotateRadius, radius, time);
            return _rotateTween;
        }

        Tween ChangeRotateHeight(float height, float time) {
            _heightTween?.Kill();
            _heightTween = DOTween.To((x) => RotateData.RotateHeight = x, RotateData.RotateHeight, height, time);
            return _heightTween;
        }
        
        void StopParentRotation() {
            if(RotateData._parentRotateRoutine != null) {
                _parent.StopCoroutine(RotateData._parentRotateRoutine);
                RotateData._parentRotateRoutine = null;
            }
        }

        void StopObjectRotation() {
            if(RotateData._objectsRotateRoutine != null) {
                _parent.StopCoroutine(RotateData._objectsRotateRoutine);
                RotateData._objectsRotateRoutine = null;
            }
        }
        
        void StartCountdown() {
            if(_countdownRoutine != null) {
                _parent.StopCoroutine(_countdownRoutine);
            }
            _countdownRoutine = _parent.StartCoroutine(CountdownCoroutine());
        }

        void Clear() {
            GameObject.Destroy(RotateData.RotateTransform.gameObject);
            _spawnedObjects.Clear();
            _heightTween?.Kill();
            _rotateTween?.Kill();
            _shrinkTween?.Kill();
        }

        void DestroySphere() {
            Ended = true;
            StopParentRotation();
            StopObjectRotation();
            Clear();
        }


        #region IEnumerators

        IEnumerator RotateCoroutine() {
            while (true) {
                if(Ended) {
                    break;
                }
                RotateData.RotateTransform.Rotate(RotateData.RotateAxis,RotateData.RotateSpeed * Time.deltaTime);
                yield return null;
            }
        }
        
        IEnumerator MoveCoroutine() {
            while (true) {
                if(Ended) {
                    break;
                }
                RotateData.RotateRadius += RotateData.RotateSpeed * Time.deltaTime;
                yield return null;
            }
        }
        
        IEnumerator CountdownCoroutine() {
            yield return new WaitForSeconds(Duration);
            Shrink();
        }

        #endregion


    }

    [Serializable]
    public struct RotateData {

        public float SpawnRadius;
        public float RotateRadius;
        public float SpawnHeight;
        public float RotateHeight;
        public float MaxRotateSpeed;
        public float MinRotateSpeed;


        public float RotateSpeed;
        public Vector3 RotateAxis;
        public float RotateStartAngle;
        [HideInInspector] public Transform RotateTransform;
        [HideInInspector] public float _originalRotateRadius;

        public Coroutine _parentRotateRoutine;
        public Coroutine _objectsRotateRoutine;

        public void GenerateRotateTransform(MonoBehaviour parent) {
            RotateTransform = new GameObject("Rotate Transform").transform;
            RotateTransform.SetParent(parent.transform);
            RotateTransform.localPosition = Vector3.zero;
            RotateTransform.localRotation = Quaternion.identity;
            RotateTransform.localScale = Vector3.one;
            _originalRotateRadius = RotateRadius;
        }
    }

}