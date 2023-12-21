using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementState : BaseState
{

	[Header("Distortion")]

	[SerializeField] private Vector2 airDistortionClamp;

	[SerializeField] private float xDistortionScale;
	[SerializeField] private float xDistortionTime;
	[SerializeField] private AnimationCurve xDistortionCurve;

	private float curxDistortionTime;

	[Header("Movement")]

	[SerializeField] private float moveSpeed;

	[SerializeField] private float jumpStrength;

	// Camadas que representam ch�o onde o jogador pode pular
	[SerializeField] private LayerMask groundMask;

	private int input;

	[HideInInspector] public Rigidbody2D rb;
	[HideInInspector] public BoxCollider2D collider;

	private Transform spriteTrs;

	public override void Enter()
	{
		// Procura um Rigidbody2D no Game Object, e atribui seu valor a vari�vel
		rb = GetComponent<Rigidbody2D>();

		// Procura um BoxCollider2D no Game Object, e atribui seu valor a vari�vel
		collider = GetComponent<BoxCollider2D>();

		spriteTrs = transform.GetChild(0);
	}
	public override void Exit()
	{
	}

	public override void Step()
	{
		input = InputController.moveAxis.GetValRaw();
		#region
		/*
		
		if (Grounded())
		{
			if (Mathf.Abs(rb.velocity.sqrMagnitude) >= 0.1f)
			{
				if (curxDistortionTime < 1f)
				{
					curxDistortionTime += Time.deltaTime * xDistortionTime;
				}
				float val = xDistortionCurve.Evaluate(curxDistortionTime) * xDistortionScale;

				Vector2 scale = new (1f + val, 1f - val);
				spriteTrs.localScale = scale;
			}
			else
			{
				if (curxDistortionTime > 0f)
				{
					curxDistortionTime -= Time.deltaTime * xDistortionTime;

					float val = xDistortionCurve.Evaluate(curxDistortionTime) * xDistortionScale;

					Vector2 scale = new(1f + val, 1f - val);
					spriteTrs.localScale = scale;
				}
				else
				{
					spriteTrs.localScale = new(1f, 1f);
				}
			}
		}
		else
		{

			Vector2 scale = new
			(1f - Mathf.Abs(rb.velocity.y) / airDistortionClamp.y, 1f + Mathf.Abs(rb.velocity.y) / airDistortionClamp.y);
			spriteTrs.localScale = scale;
		} 
		*/
		#endregion
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
