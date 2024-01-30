using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChainRollerConnector : BaseConnector
{
	[SerializeField] private List<ConnectionComponent> outputs;
	[SerializeField] private AnimationCurve colorInterpolationCurve;
	[SerializeField] private float interpolationTime;
	public float systemSpeed;

	private SpriteRenderer overlay;
	private new Light2D light;

	private void Start()
	{
		overlay = Utils.SearchObjectWithComponent<SpriteRenderer>(transform, "Overlay");
		light = Utils.SearchObjectWithComponent<Light2D>(transform, "Light");
		light.intensity = 0f;

		foreach(ConnectionComponent output in outputs)
		{
			output.SetConnection(transform);
		}

		overlay.color = new Color(
			1f, 1f, 1f,
			0f
		);
	}

	private void Update()
	{
		
	}

	public override void SetSignal(bool inputVal)
	{
		foreach(ConnectionComponent output in outputs)
		{
			output.SetSignal(inputVal);
		}
	}

	public override void SetInterpolationValue(float value)
	{
		overlay.color = new Color(
			1f, 1f, 1f,
			value
		);
		light.intensity = value;
	}
}
