using UnityEngine;

namespace Bohemia
{
    internal sealed class Game : MonoBehaviour
    {
        [SerializeField] private Unit[] _units;

        private readonly EachWithEachVisibilityDetector _visibilityDetector = new();

        private void Awake()
        {
            foreach (var unit in _units)
            {
                unit.Init();
                _visibilityDetector.TryAdd(unit);
            }
        }

        private void Update()
        {
            _visibilityDetector.Update();
        }
    }
}