using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que controla estados em uma state machine
/// </summary>
public class StateMachine: MonoBehaviour
{
	[HideInInspector] public Dictionary<string, BaseState> stateList;
	[HideInInspector] public string currentState;

	private void Awake()
	{
		stateList = new();
		currentState = "";
	}

	public void ChangeState(string newStateName)
	{
		BaseState newState = stateList[newStateName];
		if (currentState != "")
		{
			stateList[currentState].Exit();
			stateList[currentState].active = false;
		}
		currentState = newStateName;
		stateList[newStateName].Enter();
		stateList[currentState].active = true;
	}

	public void RegisterState(string stateName, BaseState newState)
	{
		stateList[stateName] = newState;
		stateList[stateName].machine = this;
		stateList[stateName].Init();
	}

	public void Update()
	{
		if (currentState != "")
		{
			stateList[currentState].Step();
		}
	}

	public void FixedUpdate()
	{
		if (currentState != "")
		{
			stateList[currentState].FixedStep();
		}
	}
}
