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
			stateMachine.RegisterState("Move", moveState);
			stateMachine.RegisterState("Free", freeState);
			stateMachine.ChangeState("Move");
		}
	}
}
