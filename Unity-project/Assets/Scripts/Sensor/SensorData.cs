using UnityEngine;

namespace Bohemia
{
    internal struct SensorData
    {
        internal Vector3 Position;
        internal Vector3 Direction;

        internal SensorData(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}