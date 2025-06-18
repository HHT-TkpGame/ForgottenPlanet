using UnityEngine;

public class ITransformStrategy : MonoBehaviour
{
    public interface ITransformSenderStrategy
    {
        void Initialize();
    }
    public interface ITransformGetterStrategy
    {
        void Initialize();
        Vector3 Pos { get; }
        float RotY { get; }
    }
}
