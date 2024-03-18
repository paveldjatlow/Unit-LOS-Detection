using UnityEngine;

namespace Bohemia
{
    /// <summary>
    /// Holds the color of the team flag
    /// </summary>
    internal sealed class UnitTeamFlagView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;

        internal void SetTeam(Color color)
        {
            _renderer.material.color = color;
        }
    }
}