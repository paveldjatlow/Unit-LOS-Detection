using UnityEngine;
using Random = UnityEngine.Random;

namespace Bohemia
{
    /// <summary>
    /// Moves all units to random positions on the map and makes them look at the destination
    /// </summary>
    internal sealed class UnitMovement
    {
        private readonly float _movementSpeed;
        private readonly Transform _unitTransform;
        private readonly Vector2 _worldSize;

        private Vector3 _destination;
        private bool _isMoving;

        internal UnitMovement(Unit unit, float movementSpeed, Vector2 worldSize)
        {
            _movementSpeed = movementSpeed;
            _unitTransform = unit.Transform;
            _worldSize = worldSize;

            SetDestination();
        }

        internal void Move()
        {
            if (_isMoving == false)
                return;

            var currentPosition = _unitTransform.position;
            var positionDifference = _destination - currentPosition;

            var direction = positionDifference.normalized;
            var distanceToDestination = positionDifference.magnitude;

            if (distanceToDestination > 1.0f)
            {
                var newPosition = currentPosition + direction;
                var step = _movementSpeed * Time.deltaTime;

                // Move to the new position
                _unitTransform.position = Vector3.MoveTowards(currentPosition, newPosition, step);

                // Look at the destination (face the direction of movement)
                _unitTransform.LookAt(_destination);
            }
            else
            {
                _unitTransform.position = _destination;
                _isMoving = false;

                SetDestination();
            }
        }

        /// <summary>
        /// Set random destination for the unit
        /// </summary>
        private void SetDestination()
        {
            var randomX = Random.Range(0, _worldSize.x);
            var randomZ = Random.Range(0, _worldSize.y);

            _destination = new Vector3(randomX, 0f, randomZ);
            _isMoving = true;
        }
    }
}