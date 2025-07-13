using UnityEngine;
public interface ITransformProvider
{
    Vector3 AgentPos { get; }
    float AgentRotY { get; }
}
