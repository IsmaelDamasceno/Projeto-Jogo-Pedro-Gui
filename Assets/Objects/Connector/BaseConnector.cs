using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

public abstract class BaseConnector: MonoBehaviour
{
	public abstract void SetSignal(bool inputVal);
	public abstract void SetInterpolationValue(float value);
}
