using System;
using System.Collections.Generic;

namespace Bohemia
{
    /// <summary>
    /// Detects visibility between units and obstacles and updates their visibility state
    /// </summary>
    internal abstract class VisibilityDetector : IDisposable
    {
        /// <summary>
        /// Pairs of units that are visible in the last frame
        /// </summary>
        private readonly HashSet<int> _lastFramePairs = new();

        /// <summary>
        /// Pairs of units that are visible during the current frame
        /// </summary>
        private readonly HashSet<int> _currentFramePairs = new();

        internal UnitStorage Storage { get; private set; }

        protected VisibilityDetector(UnitStorage storage)
        {
            Storage = storage;
        }

        public void Update()
        {
            Run();
            UpdatePairs();
        }

        /// <summary>
        /// Move the current frame pairs to the last frame pairs and clear the current frame pairs
        /// </summary>
        private void UpdatePairs()
        {
            _lastFramePairs.Clear();

            foreach (var pair in _currentFramePairs)
                _lastFramePairs.Add(pair);

            _currentFramePairs.Clear();
        }

        /// <summary>
        /// Implementation-specific logic of the visibility detector
        /// </summary>
        protected abstract void Run();

        /// <summary>
        /// Test if the unit1 can see the unit2
        /// </summary>
        /// <param name="unit1">Left unit</param>
        /// <param name="unit2">Right unit</param>
        /// <param name="removePair">Remove the pair connection after testing it</param>
        protected void Test(Unit unit1, Unit unit2, bool removePair)
        {
            if (unit1.IsIgnored(unit2))
                return;

            var hasPair = WasVisibleBefore(unit1, unit2, removePair);
            var visibilityResult = IsVisible(unit1, unit2);

            // If unit2 is visible to unit1
            if (visibilityResult)
            {
                // Currently visible now
                SetCurrentFrameVisibility(unit1, unit2);

                unit2.SetDetected(true);
                unit1.Sensor.AddDetected(unit2);
            }
            else if (hasPair)
            {
                unit2.SetDetected(false);
                unit1.Sensor.RemoveDetected(unit2);
            }
        }

        /// <summary>
        /// Was the unit visible to the sensor of the other unit in the last frame
        /// </summary>
        private bool WasVisibleBefore(Unit unit1, Unit unit2, bool remove = true)
        {
            var idx = unit1.Id * (UnitStorage.MAX_UNITS_COUNT + 1) + unit2.Id;
            return remove ? _lastFramePairs.Remove(idx) : _lastFramePairs.Contains(idx);
        }

        private void SetCurrentFrameVisibility(Unit unit1, Unit unit2)
        {
            var idx = unit1.Id * (UnitStorage.MAX_UNITS_COUNT + 1) + unit2.Id;
            _currentFramePairs.Add(idx);
        }

        /// <summary>
        /// Is the unit visible to the sensor of the other unit
        /// </summary>
        /// <param name="unit1">Left unit</param>
        /// <param name="unit2">Right unit</param>
        private static bool IsVisible(Unit unit1, Unit unit2)
        {
            return unit1.Sensor.IsVisible(unit2);
        }

        public void Dispose()
        {
            _lastFramePairs.Clear();
            _currentFramePairs.Clear();
        }
    }
}