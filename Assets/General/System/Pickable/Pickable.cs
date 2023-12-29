using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable: BaseState
{
	public bool hover;
	[SerializeField] protected LineRenderer lineRenderer;

	public abstract void PickUp();

	public override void Enter()
	{
		lineRenderer = GetComponent<LineRenderer>();
		RenderCircle.DrawCircle(lineRenderer, 16, Pickup.PickupRadius);
	}
	public override void Step()
	{
		lineRenderer.enabled = hover;
	}
	public override void FixedStep()
	{

	}
	public override void Exit()
	{

	}
}
