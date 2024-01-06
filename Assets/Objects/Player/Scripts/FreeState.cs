using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class FreeState : BaseState
	{

		[HideInInspector] public Rigidbody2D rb;
		[SerializeField] private float waitTime;

		public override void Enter()
		{
			rb = GetComponent<Rigidbody2D>();

			rb.freezeRotation = false;
			StartCoroutine(ExitCoroutine());
		}

		public override void Exit()
		{
			rb.freezeRotation = true;
		}

		IEnumerator ExitCoroutine()
		{
			yield return new WaitForSeconds(waitTime);
			machine.ChangeState("Move");
		}

		public override void Step()
		{

		}

		public override void FixedStep()
		{

		}
	}
}
