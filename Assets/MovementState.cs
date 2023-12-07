using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MovementState : BaseState
{
	[SerializeField] private float moveSpeed;

	[SerializeField] private float jumpStrength;

	// Camadas que representam ch�o onde o jogador pode pular
	[SerializeField] private LayerMask groundMask;

	private int input;

	[HideInInspector] public Rigidbody2D rb;
	[HideInInspector] public BoxCollider2D collider;

	public override void Enter()
	{
		// Procura um Rigidbody2D no Game Object, e atribui seu valor a vari�vel
		rb = GetComponent<Rigidbody2D>();

		// Procura um BoxCollider2D no Game Object, e atribui seu valor a vari�vel
		collider = GetComponent<BoxCollider2D>();

		Debug.Log("Enter Movement state");
	}
	public override void Exit()
	{
		Debug.Log("Exit Movement state");
	}

	public override void Step()
	{
		input = InputController.moveAxis.GetValRaw();
	}

	public override void FixedStep()
	{
		rb.velocity = new Vector2(input * moveSpeed, rb.velocity.y);

		if (InputController.GetKey("Jump") && Grounded())
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
		}
	}

	private bool Grounded()
	{
		return Physics2D.BoxCast(
		transform.position, (new Vector2(collider.size.x - 0.1f, collider.size.y)) * transform.localScale, 0f, Vector2.down, 0.05f, groundMask);
	}
}
