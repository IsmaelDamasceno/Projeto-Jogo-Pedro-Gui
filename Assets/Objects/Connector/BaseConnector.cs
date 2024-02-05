using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseConnector: MonoBehaviour
{
	public abstract void SetSignal(bool inputVal);
	public abstract void SetInterpolationValue(float value);
}
