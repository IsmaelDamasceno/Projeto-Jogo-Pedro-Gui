using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ConnectorOutput : MonoBehaviour
{
    [SerializeField] private ConnectionComponentOutput componentOutput;

	private SpriteRenderer overlay;
	private new Light2D light;

	private void Start()
	{
		overlay = Utils.SearchObjectWithComponent<SpriteRenderer>(transform, "Overlay");
		light = Utils.SearchObjectWithComponent<Light2D>(transform, "Light");
		light.intensity = 0f;

		overlay.color = new Color(
			1f, 1f, 1f,
			0f
		);
	}

	public void ReceiveOutput(bool signalVal)
	{
		Debug.Log($"output connector received {signalVal} signal");
		componentOutput.SetSignal(signalVal);
	}

	public void SetInterpolationValue(float value)
	{
		overlay.color = new Color(
			1f, 1f, 1f,
			value
		);
		light.intensity = value;
	}
}
