using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bohemia
{
    /// <summary>
    /// Custom unity-specific quadtree implementation
    /// Sources used as references:
    /// https://github.com/awteeter/UnityQuadTreeExample
    /// https://medium.com/analytics-vidhya/quad-tree-in-unity-794382cd74b4
    /// https://en.wikipedia.org/wiki/Quadtree
    /// </summary>
    internal sealed class QuadTree
    {
        internal Rect Bounds;

        /// <summary>
        /// How many bodies can be in the node before it splits
        /// </summary>
        internal int MaxBodiesPerNode;

        /// <summary>
        /// Max level of the quadtree
        /// </summary>
        internal int MaxLevel;

        /// <summary>
        /// Current level of the quadtree
        /// </summary>
        internal int CurrentLevel;

        private readonly List<Unit> _units;
        private int _unitsCount;

        private QuadTree _childA;
        private QuadTree _childB;
        private QuadTree _childC;
        private QuadTree _childD;

        private readonly List<Unit> _entCache = new(64);

        internal QuadTree(Rect bounds, int maxBodiesPerNode, int maxLevel)
        {
            Bounds = bounds;
            MaxBodiesPerNode = maxBodiesPerNode;
            MaxLevel = maxLevel;
            _units = new List<Unit>(maxBodiesPerNode);
        }

        internal QuadTree(Rect bounds, QuadTree parent) : this(bounds, parent.MaxBodiesPerNode, parent.MaxLevel)
        {
            CurrentLevel = parent.CurrentLevel + 1;
        }

        internal void Add(Unit unit)
        {
            if (_childA != null)
            {
                var child = GetQuadrant(unit.Position2D);
                child.Add(unit);
            }
            else
            {
                _units.Add(unit);
                _unitsCount++;

                if (_unitsCount > MaxBodiesPerNode && CurrentLevel < MaxLevel)
                {
                    Split();
                }
            }
        }

        internal List<Unit> GetUnits(Vector3 point, float radius)
        {
            var p = new Vector2(point.x, point.z);
            return GetUnits(p, radius);
        }

        private List<Unit> GetUnits(Vector2 point, float radius)
        {
            _entCache.Clear();
            GetUnits(point, radius, _entCache);
            return _entCache;
        }

        /// <summary>
        /// Get units in the circle
        /// </summary>
        private void GetUnits(Vector2 point, float radius, List<Unit> bods)
        {
            // If no children
            if (_childA == null)
            {
                bods.AddRange(_units);
            }
            else
            {
                if (_childA.ContainsCircle(point, radius))
                    _childA.GetUnits(point, radius, bods);
                if (_childB.ContainsCircle(point, radius))
                    _childB.GetUnits(point, radius, bods);
                if (_childC.ContainsCircle(point, radius))
                    _childC.GetUnits(point, radius, bods);
                if (_childD.ContainsCircle(point, radius))
                    _childD.GetUnits(point, radius, bods);
            }
        }

        /// <summary>
        /// Does the quadtree contain the circle?
        /// </summary>
        private bool ContainsCircle(Vector2 circleCenter, float radius)
        {
            var center = Bounds.center;

            var dx = Math.Abs(circleCenter.x - center.x);
            var dy = Math.Abs(circleCenter.y - center.y);

            var halfWidth = Bounds.width * 0.5f;
            var halfHeight = Bounds.height * 0.5f;

            if (dx > halfWidth + radius)
                return false;

            if (dy > halfHeight + radius)
                return false;

            if (dx <= halfWidth)
                return true;

            if (dy <= halfHeight)
                return true;

            var cornerDist = (dx - halfWidth * dx - halfWidth) + (dy - halfHeight * dy - halfHeight);
            return cornerDist <= radius * radius;
        }

        internal void DrawGizmos()
        {
            // Draw gizmos of the children
            _childA?.DrawGizmos();
            _childB?.DrawGizmos();
            _childC?.DrawGizmos();
            _childD?.DrawGizmos();

            // Draw rect gizmos
            Gizmos.color = Color.cyan;

            var p1 = new Vector3(Bounds.position.x, 0.1f, Bounds.position.y);
            var p2 = new Vector3(p1.x + Bounds.width, 0.1f, p1.z);
            var p3 = new Vector3(p1.x + Bounds.width, 0.1f, p1.z + Bounds.height);
            var p4 = new Vector3(p1.x, 0.1f, p1.z + Bounds.height);

            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);
        }

        private void Split()
        {
            var hx = Bounds.width / 2;
            var hz = Bounds.height / 2;
            var sz = new Vector2(hx, hz);

            //split a
            var aLoc = Bounds.position;
            var aRect = new Rect(aLoc, sz);
            //split b
            var bLoc = new Vector2(Bounds.position.x + hx, Bounds.position.y);
            var bRect = new Rect(bLoc, sz);
            //split c
            var cLoc = new Vector2(Bounds.position.x + hx, Bounds.position.y + hz);
            var cRect = new Rect(cLoc, sz);
            //split d
            var dLoc = new Vector2(Bounds.position.x, Bounds.position.y + hz);
            var dRect = new Rect(dLoc, sz);

            //assign QuadTrees
            _childA = QuadTreePool.GetQuadTree(aRect, this);
            _childB = QuadTreePool.GetQuadTree(bRect, this);
            _childC = QuadTreePool.GetQuadTree(cRect, this);
            _childD = QuadTreePool.GetQuadTree(dRect, this);

            var newUnitsCount = _unitsCount;

            for (int i = _unitsCount - 1; i >= 0; i--)
            {
                var unit = _units[i];

                var child = GetQuadrant(unit.Position2D);
                child.Add(unit);

                _units.Remove(unit);
                newUnitsCount--;
            }

            _unitsCount = newUnitsCount;
        }

        /// <summary>
        /// Get the quadrant of the point
        /// </summary>
        /// <param name="point">Center point</param>
        private QuadTree GetQuadrant(Vector2 point)
        {
            var halfWidth = Bounds.width * 0.5f;
            var halfHeight = Bounds.height * 0.5f;

            if (point.x > Bounds.x + halfWidth)
                return point.y > Bounds.y + halfHeight ? _childC : _childB;

            return point.y > Bounds.y + halfHeight ? _childD : _childA;
        }

        internal void Clear()
        {
            QuadTreePool.PoolQuadTree(_childA);
            QuadTreePool.PoolQuadTree(_childB);
            QuadTreePool.PoolQuadTree(_childC);
            QuadTreePool.PoolQuadTree(_childD);

            _childA = null;
            _childB = null;
            _childC = null;
            _childD = null;

            _units.Clear();
            _unitsCount = 0;
        }
    }
}