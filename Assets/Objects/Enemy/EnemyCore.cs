using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
	/// <summary>
	/// Core da state machine do inimigo
	/// </summary>
	public class EnemyCore : MonoBehaviour
	{
		public StateMachine stateMachine;
		void Start()
		{
			stateMachine = GetComponent<StateMachine>();

			EnemyMoveState moveState = GetComponent<EnemyMoveState>();
			EnemyFreeState freeState = GetComponent<EnemyFreeState>();
			stateMachine.RegisterState("Move", moveState);
			stateMachine.RegisterState("Free", freeState);
			stateMachine.ChangeState("Move");
		}

		private void Update()
		{
		}
	}
}
