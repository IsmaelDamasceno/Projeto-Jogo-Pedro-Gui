using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class FreeState : BaseState
	{
		[SerializeField] private PhysicsMaterial2D moveMaterial;
		[SerializeField] private PhysicsMaterial2D freeMaterial;
		[SerializeField] private float waitTime;

		private Rigidbody2D rb;
		private Animator animator;
		private BoxCollider2D boxCollider;
		private CircleCollider2D circleCollider;

		public override void Enter()
		{
			rb = GetComponent<Rigidbody2D>();
			animator = GetComponent<Animator>();
			boxCollider = GetComponent<BoxCollider2D>();
			circleCollider = GetComponent<CircleCollider2D>();

			circleCollider.enabled = true;
			boxCollider.enabled = false;
			
			rb.freezeRotation = false;
			rb.sharedMaterial = freeMaterial;

			animator.SetBool("Damaged", true);

			StartCoroutine(ExitCoroutine());
		}

		public override void Exit()
		{
			rb.freezeRotation = true;
			rb.sharedMaterial = moveMaterial;

			animator.SetBool("Damaged", false);

			circleCollider.enabled = false;
			boxCollider.enabled = true;
			transform.rotation = Quaternion.identity;
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
