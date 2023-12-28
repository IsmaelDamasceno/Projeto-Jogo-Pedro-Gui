using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable: MonoBehaviour
{
	public bool hover;
	[SerializeField] protected LineRenderer lineRenderer;

	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		RenderCircle.DrawCircle(lineRenderer, 16, Pickup.PickupRadius);
		Debug.Log($"Turning into circle: {Pickup.PickupRadius}");
	}
	private void Update()
	{
		lineRenderer.enabled = hover;
	}

	public abstract void PickUp();
}
