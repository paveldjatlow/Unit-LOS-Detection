using Vector3 = UnityEngine.Vector3;

namespace Bohemia
{
    public interface IDetectable
    {
        Vector3 Position { get; }
        void SetDetected(bool value);
    }
}