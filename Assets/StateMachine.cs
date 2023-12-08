using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		}
		currentState = newStateName;
		stateList[newStateName].Enter();
	}

	public void RegisterState(string stateName, BaseState newState)
	{
		stateList[stateName] = newState;
		stateList[stateName].machine = this;
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
