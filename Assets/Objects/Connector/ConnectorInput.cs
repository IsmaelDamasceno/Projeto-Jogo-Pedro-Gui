using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ConnectorInput : MonoBehaviour
{
	[SerializeField] private Cable cable;

	[SerializeField] private AnimationCurve colorInterpolationCurve;
	[SerializeField] private float interpolationTime;

	private SpriteRenderer overlay;
	private new Light2D light;

	private float currentTIme = 0;
	private bool interpolating = false;
	private bool signal;

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

	private void Update()
	{
		if (!interpolating)
		{
			return;
		}

		currentTIme += Time.deltaTime * (signal ? 1f : -1f);
		if ((currentTIme >= interpolationTime && signal == true) || (currentTIme <= 0f && signal == false))
		{
			interpolating = false;
		}
		float percent = currentTIme / interpolationTime;
		SetInterpolationValue(percent);
		overlay.color = new Color(
			1f, 1f, 1f,
			percent
		);
		light.intensity = percent;
	}

	public void ConnectCable(Cable cable)
	{
		this.cable = cable;
	}

	public void ReceiveInput(bool inputVal)
	{
		Debug.Log($"input connector received {inputVal} signal");
		cable.SetSignal(inputVal);

		signal = inputVal;
		interpolating = true;
	}

	public void SetInterpolationValue(float value)
	{
		cable.SetInterpolationValue(value);
	}
}
