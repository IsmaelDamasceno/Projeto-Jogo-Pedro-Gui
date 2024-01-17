using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pick
{
	/// <summary>
	/// Core dos objetos peg�veis
	/// </summary>
	public class PickableCore : MonoBehaviour
	{
		public StateMachine stateMachine;

		void Start()
		{
			stateMachine = GetComponent<StateMachine>();

			Pickable pickable = GetComponent<Pickable>();
			PickableHeldState held = GetComponent<PickableHeldState>();

			stateMachine.RegisterState("Pickable", pickable);
			stateMachine.RegisterState("Held", held);
			stateMachine.ChangeState("Pickable"); 
		}
	}
}
