using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState: MonoBehaviour
{

	public StateMachine machine;
	public bool active = true;

	public abstract void Enter();
	public abstract void Step();
	public abstract void FixedStep();
	public abstract void Exit();
}
