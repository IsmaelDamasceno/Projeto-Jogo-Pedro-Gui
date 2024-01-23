using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionComponentOutput : ConnectionComponent
{
    public override void SetSignal(bool signalVal)
    {
        Debug.Log($"received signal: {signalVal}");
    }
	public override void SetInterpolationValue(float value)
	{

	}
	public override void SetConnection(Transform connectionTrs)
	{

	}
}
