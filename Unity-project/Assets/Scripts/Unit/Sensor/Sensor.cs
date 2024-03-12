using UnityEngine;

namespace Bohemia
{
    public sealed class Sensor : CachedTransformObject
    {
        [SerializeField] private LayerMask _layerMask;

        [field: SerializeField] public Transform RaycastPoint { get; private set; }

        public float ViewRadius { get; private set; }
        public float ViewAngle { get; private set; }

        internal void Init(float viewRadius, float viewAngle)
        {
            ViewRadius = viewRadius;
            ViewAngle = viewAngle;
        }

        public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobal)
        {
            if (isGlobal == false)
                angleInDegrees += Transform.eulerAngles.y;

            var x = Mathf.Sin(angleInDegrees * Mathf.Deg2Rad);
            var z = Mathf.Cos(angleInDegrees * Mathf.Deg2Rad);

            return new Vector3(x, 0, z);
        }

        // internal void FindVisibleTargets()
        // {
        //     foreach (var visible in VisibleUnits)
        //         visible.SetDetected(false);
        //
        //     VisibleUnits.Clear();
        //
        //     var results = new Collider[10];
        //     var size = Physics.OverlapSphereNonAlloc(RaycastPoint.position, ViewRadius, results);
        //
        //     for (var i = 0; i < size; i++)
        //     {
        //         var target = results[i].transform;
        //         var positionDifference = target.position - RaycastPoint.position;
        //
        //         var directionToTarget = positionDifference.normalized;
        //
        //         if (Vector3.Angle(Transform.forward, directionToTarget) >= ViewAngle * 0.5f)
        //             continue;
        //
        //         var distanceToTarget = positionDifference.magnitude;
        //         var obstaclesLayerMask = LayerMask.GetMask("Obstacles");
        //
        //         if (Physics.Raycast(RaycastPoint.position, directionToTarget, distanceToTarget, obstaclesLayerMask))
        //             continue;
        //
        //         if (target.TryGetComponent(out Unit unit) == false)
        //             continue;
        //
        //         unit.SetDetected(true);
        //         VisibleUnits.Add(unit);
        //     }
        // }

        internal bool IsVisible(Unit target)
        {
            var positionDifference = target.Position - RaycastPoint.position;
            var directionToTarget = positionDifference.normalized;

            if (Vector3.Angle(Transform.forward, directionToTarget) >= ViewAngle * 0.5f)
                return false;

            var distanceToTarget = positionDifference.magnitude;

            if (distanceToTarget > ViewRadius)
                return false;

            var hasObstacleHit = Physics.Raycast(RaycastPoint.position, directionToTarget, distanceToTarget, _layerMask);

            return hasObstacleHit == false;
        }
    }
}