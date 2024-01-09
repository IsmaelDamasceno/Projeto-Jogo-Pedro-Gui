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
		public static PlayerCore instance;

		public static bool ivulnerable = false;

		public static void SetIvulnerable(float time)
		{
			instance.StopAllCoroutines();
			instance.StartCoroutine(instance.Ivulnerability(time));
		}
		private IEnumerator Ivulnerability(float time)
		{
			ivulnerable = true;
			yield return new WaitForSeconds(time);
			ivulnerable = false;
		}

#if UNITY_EDITOR
		private bool slow;
#endif

		void Start()
		{
			if (instance == null)
			{
				stateMachine = GetComponent<StateMachine>();

				BaseState moveState = GetComponent<MovementState>();
				FreeState freeState = GetComponent<FreeState>();
				DownDashState downDashState = GetComponent<DownDashState>();
				stateMachine.RegisterState("Move", moveState);
				stateMachine.RegisterState("Free", freeState);
				stateMachine.RegisterState("DownDash", downDashState);
				stateMachine.ChangeState("Move");

				instance = this;
			}
			else
			{
				Destroy(gameObject);
				return;
			}

			ivulnerable = false;
			StopAllCoroutines();
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F1))
			{
				slow = !slow;
			}
			Application.targetFrameRate = slow ? 10 : -1;
		}
#endif
	}
}
