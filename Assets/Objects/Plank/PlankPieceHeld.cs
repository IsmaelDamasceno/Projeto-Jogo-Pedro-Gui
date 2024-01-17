using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Estado presente quando um pedaço de madeira está segundo segurado pelo jogador
/// </summary>
public class PlankPieceHeld : PickableHeldState
{
    private new BoxCollider2D collider;
    private LineRenderer lineRenderer;

	public override void Enter()
    {
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.freezeRotation = true;
        rb.isKinematic = true;

        collider = GetComponent<BoxCollider2D>();
		collider.enabled = false;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

		transform.localPosition = new(0f, 0.244f);
		transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
	}

    public override void Exit()
    {
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		rb.isKinematic = false;
		rb.freezeRotation = false;

		collider.enabled = true;
		lineRenderer.enabled = true;
	}

    public override void FixedStep()
    {

    }

    public override void Step()
    {

    }
}
