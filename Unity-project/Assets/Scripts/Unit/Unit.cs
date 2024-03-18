using System;
using UnityEngine;

namespace Bohemia
{
    public sealed class Unit : CachedTransformObject
    {
        [field: SerializeField] public Sensor Sensor { get; private set; }

        [SerializeField] private UnitSettings _settings;
        [SerializeField] private UnitView _view;

        internal int Id;
        private int _team;

        private bool _isDetected;

        public Vector3 Position => Transform.position;
        internal Vector2 Position2D => new(Position.x, Position.z);

        internal UnitMovement Movement { get; private set; }

        internal void Init(EUnitTeam team, Vector3 position, Transform parent, Vector2 worldSize)
        {
            _team = (int)team;

            Transform.SetParent(parent);
            Transform.localPosition = position;

            Sensor.Init(_settings.ViewRadius, _settings.ViewAngle);
            Movement = new UnitMovement(this, _settings.MovementSpeed, worldSize);

            InitFlag(team);
        }

        /// <summary>
        /// The id is set by the unit storage
        /// </summary>
        internal void SetId(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Sets the visual flag of the unit
        /// </summary>
        private void InitFlag(EUnitTeam team)
        {
            var flagColor = team switch
            {
                EUnitTeam.Red => _settings.RedTeamColor,
                EUnitTeam.Green => _settings.GreenTeamColor,
                EUnitTeam.Blue => _settings.BlueTeamColor,
                _ => throw new ArgumentOutOfRangeException(nameof(team), team, null)
            };

            _view.Init(flagColor, _settings.DefaultColor, _settings.VisibleColor);
        }

        internal void SetDetected(bool value)
        {
            if (_isDetected == value)
                return;

            _isDetected = value;

            _view.SetAsDetected(value);
        }

        /// <summary>
        /// Should the compared unit be ignored by the sensor
        /// </summary>
        internal bool IsIgnored(Unit other)
        {
            if (other.Id == Id)
                return true;

            return other._team == _team;
        }
    }
}