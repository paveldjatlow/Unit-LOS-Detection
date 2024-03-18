namespace Bohemia
{
    /// <summary>
    /// Tries to detect visibility between each pair of units
    /// Note: this is the slowest detector and used for display how slow the naive approach is
    /// </summary>
    internal sealed class EachWithEachVisibilityDetector : VisibilityDetector
    {
        internal EachWithEachVisibilityDetector(UnitStorage storage) : base(storage)
        {
        }

        protected override void Run()
        {
            var allUnits = Storage.AllUnits;
            var count = allUnits.Count;

            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < count; j++)
                {
                    var unit1 = allUnits[i];
                    var unit2 = allUnits[j];

                    Test(unit1, unit2, true);
                }
            }
        }
    }
}