namespace Bohemia
{
    internal sealed class EachWithEachVisibilityDetector : VisibilityDetector
    {
        protected override void Run()
        {
            var count = AllUnits.Count;

            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < count; j++)
                    Test(AllUnits[i], AllUnits[j], true);
            }
        }
    }
}