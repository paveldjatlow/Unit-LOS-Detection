using System;
using Cysharp.Threading.Tasks;

namespace Bohemia
{
    internal sealed class UnitsMover
    {
        private readonly UnitStorage _storage;

        internal UnitsMover(UnitStorage storage)
        {
            _storage = storage;
        }

        internal async UniTask Update()
        {
            foreach (var (_, unit) in _storage.AllUnits)
                Move(unit).HandleExceptions();

            await UniTask.Yield();
        }

        private static async UniTask Move(Unit unit)
        {
            // A random delay to make the units move at different times
            var randomDelay = UnityEngine.Random.Range(0, 100f);

            await UniTask.Delay(TimeSpan.FromMilliseconds(randomDelay));

            unit.Movement.Move();
        }
    }
}