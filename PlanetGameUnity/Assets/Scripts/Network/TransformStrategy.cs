using UnityEngine;

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
