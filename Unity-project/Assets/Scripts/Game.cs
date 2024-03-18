using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bohemia
{
    internal sealed class Game : MonoBehaviour
    {
        [SerializeField] private UnitSpawner[] _unitSpawners;
        [SerializeField] private UnitSettings _unitSettings;

        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        [SerializeField] private Vector2 _worldSize = new(3000, 3000);

        [Tooltip("How many bodies can be in one node before it splits")]
        [SerializeField] private int _bodiesPerNode = 6;

        [Tooltip("How many times the quad tree can split")]
        [SerializeField] private int _maxSplits = 6;

        [SerializeField] private float _visibilityDetectorTick = 0.5f;
        [SerializeField] private float _quadTreeTick = 1f;
        [SerializeField] private float _unitsMoverTick = 0.5f;

        private readonly UnitStorage _unitStorage = new();
        private UnitsMover _unitsMover;

        private QuadTree _quadTree;
        private VisibilityDetector _visibilityDetector;

        private void Awake()
        {
            _unitsMover = new UnitsMover(_unitStorage);

            _quadTree = new QuadTree(new Rect(0, 0, _worldSize.x, _worldSize.y), _bodiesPerNode, _maxSplits);
            _visibilityDetector = new QuadTreeVisibilityDetector(_unitStorage, _quadTree, _unitSettings.ViewRadius);

            SpawnUnits();

            UpdateVisibilityDetector().HandleExceptions();
            UpdateQuadTree().HandleExceptions();
            UpdateUnitsMover().HandleExceptions();

            _obstacleSpawner.Spawn(_worldSize);
        }

        private void SpawnUnits()
        {
            foreach (var unitSpawner in _unitSpawners)
                unitSpawner.SpawnUnits(_unitStorage, _quadTree, _worldSize);
        }

        /// <summary>
        /// Process the visibility detector that checks if the units are visible to each other
        /// </summary>
        private async UniTask UpdateVisibilityDetector()
        {
            while (gameObject.activeSelf)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_visibilityDetectorTick));

                _visibilityDetector.Update();
            }
        }

        /// <summary>
        /// Process all units and update the quadtree
        /// </summary>
        private async UniTask UpdateQuadTree()
        {
            while (gameObject.activeSelf)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_quadTreeTick));

                _quadTree.Clear();

                foreach (var (_, unit) in _unitStorage.AllUnits)
                    _quadTree.Add(unit);
            }
        }

        /// <summary>
        /// Update the movement of the units in the storage
        /// </summary>
        private async UniTask UpdateUnitsMover()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_unitsMoverTick));

            while (gameObject.activeSelf)
            {
                await _unitsMover.Update();

                await UniTask.Delay(TimeSpan.FromSeconds(_unitsMoverTick));
            }
        }

        /// <summary>
        /// Draw the quadtree grid
        /// </summary>
        private void OnDrawGizmos()
        {
            _quadTree?.DrawGizmos();
        }

        private void OnDestroy()
        {
            _visibilityDetector.Dispose();
        }
    }
}