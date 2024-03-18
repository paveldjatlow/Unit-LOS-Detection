using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Bohemia
{
    /// <summary>
    /// A pool for quadtree objects
    /// </summary>
    internal static class QuadTreePool
    {
        private const int MAX_POOL_COUNT = 1024;
        private const int DEFAULT_MAX_BODIES_PER_NODE = 6;
        private const int DEFAULT_MAX_LEVEL = 6;

        private static Queue<QuadTree> _pool;

        private static void Init()
        {
            _pool = new Queue<QuadTree>();

            for (var i = 0; i < MAX_POOL_COUNT; i++)
                _pool.Enqueue(new QuadTree(Rect.zero, DEFAULT_MAX_BODIES_PER_NODE, DEFAULT_MAX_LEVEL));
        }

        internal static QuadTree GetQuadTree(Rect bounds, QuadTree parent)
        {
            if (_pool == null)
                Init();

            QuadTree tree;

            if (_pool!.Count > 0)
            {
                tree = _pool.Dequeue();
                tree.Bounds = bounds;
                tree.MaxLevel = parent.MaxLevel;
                tree.MaxBodiesPerNode = parent.MaxBodiesPerNode;
                tree.CurrentLevel = parent.CurrentLevel + 1;
            }
            else
            {
                tree = new QuadTree(bounds, parent);
            }

            return tree;
        }

        internal static void PoolQuadTree([CanBeNull] QuadTree tree)
        {
            if (tree == null)
                return;

            tree.Clear();

            if (_pool.Count <= MAX_POOL_COUNT)
                _pool.Enqueue(tree);
        }
    }
}