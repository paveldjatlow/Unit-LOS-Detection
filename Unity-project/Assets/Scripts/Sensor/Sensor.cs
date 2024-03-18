using System.Collections.Generic;
using UnityEngine;

namespace Bohemia
{
    public sealed class Sensor : CachedTransformObject
    {
        [SerializeField] private LayerMask _obstaclesLayerMask;

        [field: SerializeField] public Transform RaycastPoint { get; private set; }

        private readonly RaycastHit[] _raycastHits = new RaycastHit[10];

        private float _cosHalfViewAngle;

        public HashSet<Unit> DetectedUnits { get; } = new();

        public float ViewRadius { get; private set; }
        public float ViewAngle { get; private set; }

        internal void Init(float viewRadius, float viewAngle)
        {
            ViewRadius = viewRadius;
            ViewAngle = viewAngle;

            _cosHalfViewAngle = Mathf.Cos(ViewAngle * 0.5f * Mathf.Deg2Rad);
        }

        public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobal)
        {
            if (isGlobal == false)
                angleInDegrees += Transform.eulerAngles.y;

            var angleInRadians = angleInDegrees * Mathf.Deg2Rad;

            var x = Mathf.Sin(angleInRadians);
            var z = Mathf.Cos(angleInRadians);

            return new Vector3(x, 0, z);
        }

        internal bool IsVisible(Unit target)
        {
            var positionDifference = target.Position - RaycastPoint.position;
            var directionToTarget = positionDifference.normalized;

            // The Vector3.Angle method internally computes a square root, which can be expensive.
            // If we only need to compare angles without knowing their actual values, we might avoid computing
            // the actual angle and directly compare the cosine of the angle instead.

            if (Vector3.Dot(Transform.forward, directionToTarget) < _cosHalfViewAngle)
                return false;

            var distanceToTarget = positionDifference.magnitude;

            if (distanceToTarget > ViewRadius)
                return false;

            var raycastResult = Physics.RaycastNonAlloc(
                RaycastPoint.position,
                directionToTarget,
                _raycastHits,
                distanceToTarget,
                _obstaclesLayerMask);

            return raycastResult <= 0;
        }

        internal void AddDetected(Unit unit)
        {
            DetectedUnits.Add(unit);
        }

        internal void RemoveDetected(Unit unit)
        {
            DetectedUnits.Remove(unit);
        }
    }
}