using System.Collections.Generic;
using UnityEngine;

namespace Bohemia
{
    public sealed class Sensor : MonoBehaviour
    {
        [field: SerializeField] public Transform RaycastPoint { get; private set; }

        private Transform _transform => transform;

        public float ViewRadius { get; private set; }
        public float ViewAngle { get; private set; }

        public List<IDetectable> VisibleTargets { get; } = new();

        internal void Init(float viewRadius, float viewAngle)
        {
            // _transform = transform;

            ViewRadius = viewRadius;
            ViewAngle = viewAngle;
        }

        public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobal)
        {
            if (isGlobal == false)
                angleInDegrees += _transform.eulerAngles.y;

            var x = Mathf.Sin(angleInDegrees * Mathf.Deg2Rad);
            var z = Mathf.Cos(angleInDegrees * Mathf.Deg2Rad);

            return new Vector3(x, 0, z);
        }

        internal void FindVisibleTargets()
        {
            foreach (var visible in VisibleTargets)
                visible.SetDetected(false);

            VisibleTargets.Clear();

            Collider[] targets = Physics.OverlapSphere(RaycastPoint.position, ViewRadius);

            for (var i = 0; i < targets.Length; i++)
            {
                var target = targets[i].transform;
                var positionDifference = target.position - RaycastPoint.position;

                var directionToTarget = positionDifference.normalized;

                if (Vector3.Angle(_transform.forward, directionToTarget) >= ViewAngle * 0.5f)
                    continue;

                var distanceToTarget = positionDifference.magnitude;
                var obstaclesLayerMask = LayerMask.GetMask("Obstacles");

                if (Physics.Raycast(RaycastPoint.position, directionToTarget, distanceToTarget, obstaclesLayerMask))
                    continue;

                if (target.TryGetComponent(out IDetectable visible) == false)
                    continue;

                visible.SetDetected(true);
                VisibleTargets.Add(visible);
            }
        }
    }
}