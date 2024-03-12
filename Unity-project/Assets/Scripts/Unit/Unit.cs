using System;
using UnityEngine;

namespace Bohemia
{
    public sealed class Unit : CachedTransformObject, IEquatable<Unit>
    {
        [SerializeField] private UnitView _view;
        [SerializeField] private Sensor _sensor;

        private bool _isDetected;

        public Sensor Sensor => _sensor;
        public Vector3 Position => Transform.position;

        internal int Id { get; private set; }

        internal void Init()
        {
            _sensor.Init(10, 90);
        }

        internal void SetId(int id)
        {
            Id = id;
        }

        public void SetDetected(bool value)
        {
            if (_isDetected == value)
                return;

            _isDetected = value;

            _view.SetAsDetected(value);
        }

        public bool Equals(Unit other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return base.Equals(other) && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Unit other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id);
        }
    }
}