using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MovementState : BaseState
{

	[Header("Movement")]
	[SerializeField] private float moveSpeed;
	[SerializeField] private float jumpStrength;
	private float direction = 0f;

	[Header("Acceleration")]
	[SerializeField] private AnimationCurve accelerationCurve;
	[SerializeField] private float accelerationScale;

	[SerializeField] private float accelerationTimeScale;
	[SerializeField] private float decelerationTimeScale;

	private float curAccelarationTime;
	[SerializeField] private float curAcceleration;

	// Boost
	[SerializeField] private float maxBoost;
	[SerializeField] private float boostDecay;
	private float currentBoost;

	// Camadas que representam ch�o onde o jogador pode pular
	[SerializeField] private LayerMask groundMask;

	private int inputLastFrame = 0;
	private int input;

	[HideInInspector] public Rigidbody2D rb;
	[HideInInspector] public BoxCollider2D collider;

	private Transform spriteTrs;

	public void ApplyBoost(float val)
	{
		currentBoost += val;
	}
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
		if (input != 0f)
		{
			if (input == inputLastFrame)
			{
				direction = input;

				if (curAccelarationTime < 1f)
				{
					curAccelarationTime += Time.deltaTime * accelerationTimeScale;
				}
				curAcceleration = accelerationCurve.Evaluate(curAccelarationTime) * accelerationScale;
			}
			else
			{
				curAccelarationTime = 0f;
				curAcceleration = 0f;
			}
		}
		else
		{
			if (curAccelarationTime > 0f)
			{
				curAccelarationTime -= Time.deltaTime * decelerationTimeScale;
			}
			curAcceleration = accelerationCurve.Evaluate(curAccelarationTime) * accelerationScale;
		}

		inputLastFrame = input;

		if (Mathf.Abs(currentBoost) >= boostDecay)
		{
			currentBoost = Mathf.Sign(currentBoost) * boostDecay * Time.deltaTime;
		}
		else
		{
			currentBoost = 0f;
		}
	}

	public override void FixedStep()
	{
		rb.velocity = new Vector2(
			direction * ((Mathf.Abs(input) * moveSpeed) + curAcceleration) + currentBoost, rb.velocity.y);

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
