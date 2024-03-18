using UnityEngine;

namespace Bohemia
{
    /// <summary>
    /// Settings for the unit
    /// </summary>
    [CreateAssetMenu(fileName = "New Unit Settings", menuName = "Unit/Settings")]
    internal sealed class UnitSettings : ScriptableObject
    {
        /// <summary>
        /// How far the unit can see
        /// </summary>
        [field: SerializeField] public float ViewRadius { get; private set; } = 10;

        /// <summary>
        /// How wide the angle of the unit's vision is
        /// </summary>
        [field: SerializeField] public float ViewAngle { get; private set; } = 90;

        [field: SerializeField] public float MovementSpeed { get; private set; } = 2;

        [field: SerializeField] public Color DefaultColor { get; private set; }
        [field: SerializeField] public Color VisibleColor { get; private set; }

        [field: SerializeField] public Color RedTeamColor { get; private set; }
        [field: SerializeField] public Color GreenTeamColor { get; private set; }
        [field: SerializeField] public Color BlueTeamColor { get; private set; }
    }
}