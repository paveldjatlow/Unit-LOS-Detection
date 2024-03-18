using UnityEngine;

namespace Bohemia
{
    /// <summary>
    /// Spawns obstacles on the map
    /// </summary>
    internal sealed class ObstacleSpawner : CachedTransformObject
    {
        [SerializeField] private Obstacle _template;
        [SerializeField] private int _obstaclesCount;

        /// <summary>
        /// Spawn obstacles on the map with random positions in the given world size
        /// </summary>
        /// <param name="worldSize">Size of the world, 0 - x; 0 - y are the positions</param>
        internal void Spawn(Vector2 worldSize)
        {
            for (var i = 0; i < _obstaclesCount; i++)
            {
                var obstacle = Instantiate(_template);

                var x = Random.Range(0, worldSize.x);
                var y = Random.Range(0, worldSize.y);

                obstacle.Init(new Vector3(x, 0, y), Transform);
            }
        }
    }
}