using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : ConnectionComponent
{
    [SerializeField] private Connector output;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;
    [SerializeField] private AnimationCurve colorInterpolationCurve;
    [SerializeField] private float interpolationTime;

    private LineRenderer lineRenderer;

	private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.startColor = offColor;
		lineRenderer.endColor = offColor;
		lineRenderer.SetPosition(1, output.transform.position);
	}

    public override void SetSignal(bool signalVal)
    {
        Debug.Log($"Cable received {signalVal} signal");
		output.SetSignal(signalVal);
    }
	public override void SetInterpolationValue(float value)
	{
        output.SetInterpolationValue(value);

		Color currentColor = new Color(
			Mathf.Lerp(offColor.r, onColor.r, value),
			Mathf.Lerp(offColor.g, onColor.g, value),
			Mathf.Lerp(offColor.b, onColor.b, value),
			1f
		);

		lineRenderer.startColor = currentColor;
		lineRenderer.endColor = currentColor;
	}
	public override void SetConnection(Transform connectionTrs)
	{
		Debug.Log(connectionTrs.position);
		lineRenderer.SetPosition(0, connectionTrs.position);
	}
}
