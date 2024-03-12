using UnityEngine;

namespace Bohemia
{
    public class CachedTransformObject : MonoBehaviour
    {
        private Transform _transform;

        public Transform Transform
        {
            get
            {
                if (_transform == null)
                    _transform = transform;

                return _transform;
            }
        }
    }
}