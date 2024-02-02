using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
	/// <summary>
	/// Estado presente quando o jogador está sobre influência total da física de jogo, e em queda livre (por exemplo, quando sofre algum dano)
	/// </summary>
	public class FreeState : BaseState
	{
		[SerializeField] private PhysicsMaterial2D moveMaterial;
		[SerializeField] private PhysicsMaterial2D freeMaterial;
		[SerializeField] private float waitTime;

		public override void Init()
		{

		}

		public override void Enter()
		{
			PlayerCore.circleCollider.enabled = true;
			PlayerCore.boxCollider.enabled = false;
			
			PlayerCore.rb.freezeRotation = false;
			PlayerCore.rb.sharedMaterial = freeMaterial;

			PlayerCore.animator.SetBool("Damaged", true);

			StartCoroutine(ExitCoroutine());
		}

		public override void Exit()
		{
			PlayerCore.rb.freezeRotation = true;
			PlayerCore.rb.sharedMaterial = moveMaterial;

			PlayerCore.animator.SetBool("Damaged", false);

			PlayerCore.circleCollider.enabled = false;
			PlayerCore.boxCollider.enabled = true;
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
