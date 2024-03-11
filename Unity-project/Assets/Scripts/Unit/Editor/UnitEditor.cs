using UnityEditor;
using UnityEngine;

namespace Bohemia
{
    [CustomEditor(typeof(Unit))]
    internal sealed class UnitEditor : Editor
    {
        private void OnSceneGUI()
        {
            var unit = (Unit)target;
            var sensor = unit.Sensor;

            if (sensor == null)
                return;

            var unitPosition = unit.Sensor.RaycastPoint.position;
            var halfAngle = sensor.ViewAngle * 0.5f;

            var viewAngleA = sensor.DirectionFromAngle(-halfAngle, false);
            var viewAngleB = sensor.DirectionFromAngle(halfAngle, false);

            Handles.color = Color.white;
            Handles.DrawWireArc(unitPosition, Vector3.up, viewAngleA, sensor.ViewAngle, sensor.ViewRadius, 2);

            var viewAngleAWorld = unitPosition + viewAngleA * sensor.ViewRadius;
            var viewAngleBWorld = unitPosition + viewAngleB * sensor.ViewRadius;

            Handles.DrawLine(unitPosition, viewAngleAWorld);
            Handles.DrawLine(unitPosition, viewAngleBWorld);

            foreach (var visibleTarget in sensor.VisibleTargets)
            {
                Handles.color = Color.red;
                Handles.DrawLine(unitPosition, visibleTarget.Position);
            }
        }
    }
}