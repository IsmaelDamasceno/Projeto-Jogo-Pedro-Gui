using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeState : BaseState
{

	[HideInInspector] public Rigidbody2D rb;

	public override void Enter()
	{
		rb = GetComponent<Rigidbody2D>();

		rb.freezeRotation = true;
	}

	public override void Exit()
	{
		rb.freezeRotation = false;
	}

	public override void Step()
	{

	}

	public override void FixedStep()
	{

	}
}
