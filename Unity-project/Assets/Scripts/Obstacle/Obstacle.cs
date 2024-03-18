using UnityEngine;

namespace Bohemia
{
    /// <summary>
    /// Obstacle that can be placed on the map
    /// Cannot be seen through and blocks raycasts
    /// </summary>
    internal sealed class Obstacle : CachedTransformObject
    {
        internal void Init(Vector3 position, Transform parent)
        {
            Transform.SetParent(parent);
            Transform.localPosition = position;

            SetRandomRotation();
        }
    }
}