using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class NoMoveState : BaseState
	{
		public override void Enter()
		{
			PlayerCore.rb.velocity = Vector2.zero;
		}

		public override void Exit()
		{
		}

		public override void FixedStep()
		{
		}

		public override void Init()
		{
		}

		public override void Step()
		{
		}
	}
}
