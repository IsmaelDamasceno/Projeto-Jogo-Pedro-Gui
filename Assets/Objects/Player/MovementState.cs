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

	[Header("Boost")]
	[SerializeField] private AnimationCurve boostCurve;
	[SerializeField] private float boostTimeScale;
	[SerializeField] private float boostImpulseMultiplier;
	[SerializeField] private float boostBreakMultiplier;

	private float currentBoostScale;
	private float currentBoostTime;
	[SerializeField] private float currentBoost;
	private bool boosting = false;

	// Camadas que representam ch�o onde o jogador pode pular
	[SerializeField] private LayerMask groundMask;

	private int inputLastFrame = 0;
	private int input;

	[HideInInspector] public Rigidbody2D rb;
	[HideInInspector] public BoxCollider2D collider;

	private Transform spriteTrs;

	public void ApplyBoost(float boostValue)
	{
		currentBoostScale += boostValue;
		currentBoostTime = 0f;
		boosting = true;
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
		#region Base Movement
		input = InputController.moveAxis.GetValRaw();
		if (input != 0f)
		{
			if (!boosting)
			{
				direction = input;
			}

			if (input == inputLastFrame)
			{
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
		#endregion

		#region Boost Movement
		if ((currentBoost >= moveSpeed + accelerationScale || currentBoostTime <= 0.05f) && currentBoostTime <= 1f && boosting)
		{
			float multiplier = 1f;
			if (input != 0)
			{
				multiplier = input == direction ? boostImpulseMultiplier : boostBreakMultiplier;
			}

			currentBoostTime += Time.deltaTime * boostTimeScale * multiplier;
			currentBoost = boostCurve.Evaluate(currentBoostTime) * currentBoostScale;
		}
		else
		{
			currentBoostScale = 0f;
			boosting = false;
		}
		#endregion
	}

	public override void FixedStep()
	{
		if (boosting)
		{
			rb.velocity = new Vector2(direction * currentBoost, rb.velocity.y);
		}
		else
		{
			float vel = direction * ((Mathf.Abs(input) * moveSpeed) + curAcceleration);
			rb.velocity = new Vector2(vel, rb.velocity.y);
		}
		

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
