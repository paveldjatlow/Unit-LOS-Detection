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

            DrawFieldOfView(unit, sensor);
        }

        /// <summary>
        /// Draw the field of view of the unit if it's selected
        /// </summary>
        private static void DrawFieldOfView(Unit unit, Sensor sensor)
        {
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

            foreach (var visibleTarget in sensor.DetectedUnits)
            {
                Handles.color = Color.red;
                Handles.DrawLine(unitPosition, visibleTarget.Position);
            }
        }
    }
}