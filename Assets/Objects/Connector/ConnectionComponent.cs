using UnityEngine;

public abstract class ConnectionComponent : MonoBehaviour
{
	public abstract void SetSignal(bool signalVal);
	public abstract void SetInterpolationValue(float value);
	public abstract void SetConnection(Transform connectionTrs);
}
