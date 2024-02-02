using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base de todo estado presente em uma state machine
/// </summary>
public abstract class BaseState: MonoBehaviour
{
	public StateMachine machine;
	public bool active = true;

	public abstract void Init();
	public abstract void Enter();
	public abstract void Step();
	public abstract void FixedStep();
	public abstract void Exit();
}
