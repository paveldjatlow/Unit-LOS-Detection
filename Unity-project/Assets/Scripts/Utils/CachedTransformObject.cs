using UnityEngine;

namespace Bohemia
{
    /// <summary>
    /// Base class for cached transform object
    /// Contains cached transform property and SetRandomRotation method
    /// </summary>
    public class CachedTransformObject : MonoBehaviour
    {
        private Transform _transform;

        /// <summary>
        /// UnityObject.transform is a property, therefore it's better to cache it
        /// https://medium.com/@collectivemass/unity-myth-buster-gameobject-transform-vs-cached-transform-fe42a4e47491
        /// </summary>
        protected internal Transform Transform
        {
            get
            {
                if (_transform == null)
                    _transform = transform;

                return _transform;
            }
        }

        protected void SetRandomRotation()
        {
            var randomRotation = new Vector3(0, Random.Range(0, 360), 0);
            Transform.eulerAngles = randomRotation;
        }
    }
}