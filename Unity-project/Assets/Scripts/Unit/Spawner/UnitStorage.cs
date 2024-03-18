using System.Collections.Generic;

namespace Bohemia
{
    /// <summary>
    /// Collection of all units in the game
    /// I decided not to make the list static to avoid potential issues with access
    /// Instead, I will pass the reference to the storage to the classes that need it to utilize the benefits of DI
    /// </summary>
    internal sealed class UnitStorage
    {
        internal const int MAX_UNITS_COUNT = 10000;

        internal Dictionary<int, Unit> AllUnits { get; } = new(MAX_UNITS_COUNT);

        private int _currentUnitIndex;

        public void TryAdd(Unit unit)
        {
            if (AllUnits.Count >= MAX_UNITS_COUNT)
                return;

            unit.SetId(_currentUnitIndex);
            AllUnits[_currentUnitIndex] = unit;

            _currentUnitIndex++;
        }
    }
}