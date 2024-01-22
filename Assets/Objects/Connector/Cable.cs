using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    [SerializeField] private ConnectorOutput output;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;
    [SerializeField] private AnimationCurve colorInterpolationCurve;
    [SerializeField] private float interpolationTime;

    private LineRenderer lineRenderer;


	private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.startColor = offColor;
		lineRenderer.endColor = offColor;
	}

    public void SetSignal(bool signalVal)
    {
        Debug.Log($"Cable received {signalVal} signal");
        output.ReceiveOutput(signalVal);
    }
	public void SetInterpolationValue(float value)
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
}
