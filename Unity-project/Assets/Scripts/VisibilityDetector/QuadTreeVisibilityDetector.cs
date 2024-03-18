namespace Bohemia
{
    /// <summary>
    /// Detects visibility using a quad tree
    /// </summary>
    internal sealed class QuadTreeVisibilityDetector : VisibilityDetector
    {
        private readonly QuadTree _quadTree;
        private readonly float _viewRadius;

        internal QuadTreeVisibilityDetector(UnitStorage storage, QuadTree tree, float viewRadius) : base(storage)
        {
            _quadTree = tree;
            _viewRadius = viewRadius;
        }

        protected override void Run()
        {
            var allUnits = Storage.AllUnits;
            var count = allUnits.Count;

            for (var i = 0; i < count; i++)
            {
                var unit1 = allUnits[i];
                var quadTreeUnits = _quadTree.GetUnits(unit1.Position, _viewRadius);

                for (var j = 0; j < quadTreeUnits.Count; j++)
                {
                    var unit2 = quadTreeUnits[j];
                    Test(unit1, unit2, true);
                }
            }
        }
    }
}