using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCore : MonoBehaviour
{
	private StateMachine stateMachine;
	void Start()
	{
		stateMachine = GetComponent<StateMachine>();

		BaseState moveState = GetComponent<MovementState>();
		stateMachine.RegisterState("Move State", moveState);
		stateMachine.ChangeState("Move State");
	}
}
