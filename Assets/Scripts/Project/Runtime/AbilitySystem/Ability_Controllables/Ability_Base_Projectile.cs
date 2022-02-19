using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace RPGSystems.Abilities {
    public abstract class Ability_Base_Projectile {
        
        protected MonoBehaviour _parent;

        public bool Rotate = false;
        
        [HideIf("@Rotate == false")]
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
        Transform _rotateTransform;
        private float _originalRotateRadius;
        
        Coroutine _parentRotateRoutine;
        Coroutine _objectsRotateRoutine;
    }
    
}