using UnityEngine;

namespace Bohemia
{
    internal sealed class UnitView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;

        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _detectedColor;

        internal void SetAsDetected(bool value)
        {
            _renderer.material.color = value ? _detectedColor : _defaultColor;
        }
    }
}