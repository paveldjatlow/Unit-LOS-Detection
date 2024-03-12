using System.Collections.Generic;

namespace Bohemia
{
    internal abstract class VisibilityDetector
    {
        private const int MAX_UNITS_COUNT = 10000;

        protected Dictionary<int, Unit> AllUnits { get; } = new(MAX_UNITS_COUNT);

        private readonly HashSet<int> _currentPairs = new();
        private readonly HashSet<int> _nextFramePairs = new();

        private int _currentUnitIndex;

        public void TryAdd(Unit unit)
        {
            if (AllUnits.Count >= MAX_UNITS_COUNT)
                return;

            unit.SetId(_currentUnitIndex);
            AllUnits[_currentUnitIndex] = unit;

            _currentUnitIndex++;
        }

        public void Update()
        {
            Run();

            _currentPairs.Clear();

            foreach (var pair in _nextFramePairs)
                _currentPairs.Add(pair);

            _nextFramePairs.Clear();
        }

        protected abstract void Run();

        protected void Test(Unit unit1, Unit unit2, bool removePair)
        {
            if (unit1.Equals(unit2))
                return;

            var hasPair = FindVisibilityPair(unit1, unit2, removePair);

            if (IsVisible(unit1, unit2))
            {
                SetNextFrameVisibility(unit1, unit2);

                unit2.SetDetected(true);
            }
            else if (hasPair)
            {
                unit2.SetDetected(false);
            }
        }

        private bool FindVisibilityPair(Unit unit1, Unit unit2, bool remove = true)
        {
            var idx = unit1.Id * (MAX_UNITS_COUNT + 1) + unit2.Id;
            return remove ? _currentPairs.Remove(idx) : _currentPairs.Contains(idx);
        }

        private void SetNextFrameVisibility(Unit unit1, Unit unit2)
        {
            var idx = unit1.Id * (MAX_UNITS_COUNT + 1) + unit2.Id;
            _nextFramePairs.Add(idx);
        }

        private static bool IsVisible(Unit unit1, Unit unit2)
        {
            return unit1.Sensor.IsVisible(unit2);
        }
    }
}