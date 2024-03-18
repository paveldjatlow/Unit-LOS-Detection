using UnityEngine;

namespace Bohemia
{
    /// <summary>
    /// Holds the view of the unit (mesh renderer) and its team flag (color of the team)
    /// </summary>
    internal sealed class UnitView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private UnitTeamFlagView _teamFlagView;

        private Color _defaultColor;
        private Color _visibleColor;

        internal void Init(Color teamColor, Color defaultColor, Color visibleColor)
        {
            _defaultColor = defaultColor;
            _visibleColor = visibleColor;

            SetAsDetected(false);

            _teamFlagView.SetTeam(teamColor);
        }

        /// <summary>
        /// Sets the unit as detected
        /// Default: gray
        /// Detected: white
        /// </summary>
        /// <param name="value">Is detected by another unit?</param>
        internal void SetAsDetected(bool value)
        {
            _renderer.material.color = value ? _visibleColor : _defaultColor;
        }
    }
}