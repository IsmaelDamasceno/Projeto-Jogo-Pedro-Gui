using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	/// <summary>
	/// Estado presenta quando o player está sob controle do movemento do usuário por meio do teclado ou controle
	/// </summary>
	public class MovementState : BaseState
	{
		#region Movement
		[Header("Movement")]
		[SerializeField] private float moveSpeed;
		[SerializeField] private float jumpStrength;
		[SerializeField] private float jumpTime;
		public float direction = 0f;
		public bool moving = false;
		#endregion

		#region Acceleration
		[Header("Acceleration")]
		[SerializeField] private AnimationCurve accelerationCurve;
		[SerializeField] private float accelerationScale;

		[SerializeField] private float accelerationTimeScale;
		[SerializeField] private float decelerationTimeScale;

		private float curAccelarationTime;
		[SerializeField] private float curAcceleration;
		#endregion

		#region Boost
		[Header("Boost")]
		[SerializeField] private AnimationCurve boostCurve;
		[SerializeField] private float boostTimeScale;
		[SerializeField] private float boostImpulseMultiplier;
		[SerializeField] private float boostBreakMultiplier;
		[SerializeField] private float boostInvertMultiplier;

		private float currentBoostScale;
		private float currentBoostTime;
		[SerializeField] private float currentBoost;
		private bool boosting = false;
		#endregion

		// Camadas que representam ch�o onde o jogador pode pular
		[SerializeField] private LayerMask groundMask;

		private int inputLastFrame = 0;
		private int input;

		private float initialY = 0f;
		private bool startedJump = false;

		public void ApplyBoost(int direction, float boostValue)
		{
			currentBoostScale += boostValue;
			currentBoostTime = 0f;
			boosting = true;
			this.direction = direction;
		}

		public float GetMaxRegularSpeed()
		{
			return moveSpeed + accelerationScale;
		}

		public override void Enter()
		{
		}
		public override void Exit()
		{

		}

		public override void Step()
		{
			#region Base Movement
			input = InputController.moveAxis.GetValRaw();
			moving = input != 0;
			if (input != 0f)
			{
				if (!boosting)
				{
					direction = input;
					transform.localScale = new(direction, 1f, 1f);
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

			if (InputController.GetKey("Jump"))
			{
				if (PlayerCore.grounded)
				{
					PlayerCore.rb.velocity = new(PlayerCore.rb.velocity.x, jumpStrength);
					initialY = transform.position.y;
					startedJump = true;
				}
			}
			else
			{
				float yDiff = transform.position.y - initialY;
				if (!PlayerCore.grounded && yDiff >= 1.4f && PlayerCore.rb.velocity.y >= 0f && startedJump)
				{
					PlayerCore.rb.velocity = new Vector2(PlayerCore.rb.velocity.x, Mathf.Abs(PlayerCore.rb.velocity.y * 0.9f));
				}
				else if (PlayerCore.grounded)
				{
					startedJump = false;
				}
			}
			#endregion

			#region Boost Movement
			if ((input == 0f || input != direction || currentBoost >= moveSpeed + accelerationScale || currentBoostTime <= 0.05f) && currentBoostTime <= 1f && boosting)
			{
				float multiplier = 1f;
				if (currentBoost > moveSpeed + accelerationScale)
				{
					if (input != 0)
					{
						multiplier = input == direction ? boostImpulseMultiplier : boostBreakMultiplier;
					}
				}
				else
				{
					if (input != direction)
					{
						multiplier = boostInvertMultiplier;
					}
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

			#region State Change
			if (InputController.GetKeyDown("DownDash") && !PlayerCore.grounded)
			{
				machine.ChangeState("DownDash");
			}
			#endregion
		}

		public override void FixedStep()
		{
			if (boosting)
			{
				PlayerCore.rb.velocity = new Vector2(direction * currentBoost, PlayerCore.rb.velocity.y);
			}
			else
			{
				float vel = direction * ((Mathf.Abs(input) * moveSpeed) + curAcceleration);
				PlayerCore.rb.velocity = new Vector2(vel, PlayerCore.rb.velocity.y);
			}
		}
	}
}
