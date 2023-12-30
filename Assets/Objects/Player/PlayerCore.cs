using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
	public class PlayerCore : MonoBehaviour
	{
		public StateMachine stateMachine;
		void Start()
		{
			stateMachine = GetComponent<StateMachine>();

			BaseState moveState = GetComponent<MovementState>();
			FreeState freeState = GetComponent<FreeState>();
			DownDashState downDashState = GetComponent<DownDashState>();
			stateMachine.RegisterState("Move", moveState);
			stateMachine.RegisterState("Free", freeState);
			stateMachine.RegisterState("DownDash", downDashState);
			stateMachine.ChangeState("Move");
		}
	}
}
