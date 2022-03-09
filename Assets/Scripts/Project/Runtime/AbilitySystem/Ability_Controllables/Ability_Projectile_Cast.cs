using System;
using System.Collections.Generic;
using Base;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
namespace RPGSystems.Abilities {
    [Serializable]
    public class Ability_Projectile_Cast {
        [HideLabel]
        public Projectile_Cast_Data CastData;
        protected CoroutineQueue _CoroutineQueue;
        
        List<GameObject> _Projectiles = new List<GameObject>();

        public virtual void Setup(MonoBehaviour parent, Projectile_Singular_Data projectileData) {
            this.CastData.parent = parent;
            _CoroutineQueue = new CoroutineQueue(parent);
            _CoroutineQueue.StartLoop();
        }

        protected GameObject[] Cast_Projectile(Transform ProjectileParent) {
            _Projectiles.Add(ProjectileParent.gameObject);
            return Cast_Circle(ProjectileParent);
        }

        protected GameObject[] Cast_Circle(Transform p) {
            GameObject[] projectiles = new GameObject[CastData.SpawnCount];
            for (int i = 0; i < CastData.SpawnCount; i++) {
                GameObject go = GameObject.Instantiate(CastData.SpawnObjectPrefab, p);
                
                float angle =  i * Mathf.PI * 2 / CastData.SpawnCount;
                Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * CastData.SpawnRadius + CastData.SpawnOffset;
                go.transform.localPosition = pos;
                go.transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
                projectiles[i] = go;
            }
            return projectiles;
        }
        
        public void Stop() {
            _CoroutineQueue.StopLoop();
        }

        public virtual void ClearCast() {
            _CoroutineQueue.StopLoop();
            for (int i = _Projectiles.Count - 1; i >= 0; i--) {
                GameObject.Destroy(_Projectiles[i].gameObject);
            }
            _Projectiles = new List<GameObject>();
            _CoroutineQueue.StartLoop();
        }

    }
    [Serializable]
    public struct Projectile_Cast_Data {
        [HideInInspector]public MonoBehaviour parent;
        [Header("Projectile Cast Data")]
        public GameObject SpawnObjectPrefab;
        public Vector3 SpawnOrientation;
        public Vector3 SpawnOffset;
        public int SpawnCount;
        public int MaxSpawnCount;
        public int MinSpawnCount;
        public float SpawnRadius;
        public float SpawnAngle;

        public Projectile_Cast_Data([CanBeNull]bool fresh = false) {
            parent = null;
            SpawnObjectPrefab = null;
            SpawnOrientation = new Vector3(0, 0, 1);
            SpawnOffset = Vector3.zero;
            SpawnCount = 1;
            MaxSpawnCount = 1;
            MinSpawnCount = 1;
            SpawnRadius = 1;
            SpawnAngle = 0;
        }
        
        public Projectile_Cast_Data(Projectile_Cast_Data data) {
            parent = data.parent;
            SpawnObjectPrefab = data.SpawnObjectPrefab;
            SpawnOrientation = data.SpawnOrientation;
            SpawnOffset = data.SpawnOffset;
            SpawnCount = data.SpawnCount;
            MaxSpawnCount = data.MaxSpawnCount;
            MinSpawnCount = data.MinSpawnCount;
            SpawnRadius = data.SpawnRadius;
            SpawnAngle = data.SpawnAngle;
        }
    }
}