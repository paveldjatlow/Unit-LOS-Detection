using UnityEngine;

namespace Bohemia
{
    internal sealed class UnitSpawner : CachedTransformObject
    {
        [SerializeField] private Unit _template;
        [SerializeField] private EUnitTeam _team;

        [SerializeField] private int _unitsCount;
        [SerializeField] private Vector2 _spawnAreaSize;

        /// <summary>
        /// Spawn units and add them to the storage and quadtree
        /// </summary>
        /// <param name="unitStorage">Storage of the units</param>
        /// <param name="quadTree">Quad tree implementation</param>
        /// <param name="worldSize">The size of the map (0 - x, 0 - y)</param>
        internal void SpawnUnits(UnitStorage unitStorage, QuadTree quadTree, Vector2 worldSize)
        {
            for (var i = 0; i < _unitsCount; i++)
            {
                var unit = Instantiate(_template);

                var x = Random.Range(0, _spawnAreaSize.x);
                var y = Random.Range(0, _spawnAreaSize.y);

                unit.Init(_team, new Vector3(x, 0, y), Transform, worldSize);

                unitStorage.TryAdd(unit);
                quadTree.Add(unit);
            }
        }
    }
}