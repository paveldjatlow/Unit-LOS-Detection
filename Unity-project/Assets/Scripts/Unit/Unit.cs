using UnityEngine;

namespace Bohemia
{
    public sealed class Unit : MonoBehaviour, IDetectable
    {
        [SerializeField] private UnitView _view;
        [SerializeField] private Sensor _sensor;

        public Sensor Sensor => _sensor;

        // TODO: cache transform
        public Vector3 Position => transform.position;

        private void Awake()
        {
            _sensor.Init(10, 90);
        }

        private void Update()
        {
            Sensor.FindVisibleTargets();
        }

        public void SetDetected(bool value)
        {
            _view.SetAsDetected(value);
        }
    }
}