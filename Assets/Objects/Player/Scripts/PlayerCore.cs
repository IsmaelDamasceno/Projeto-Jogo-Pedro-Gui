using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
	/// <summary>
	/// Core da máquina de estado do jogador, inclui variáveis globais usadas por diferentes estados para evitar duplicar referências
	/// </summary>
	public class PlayerCore : MonoBehaviour
	{
		public static StateMachine stateMachine;
		public static PlayerCore instance;

		#region Global Variables
		public static Rigidbody2D rb;
		public static Animator animator;
		public static BoxCollider2D boxCollider;
		public static CircleCollider2D circleCollider;
		#endregion

		public static float startGravScale;
		// public static CircleGroundDetection circGroundDetection;

		#region Ivulnerability
		public static bool invulnerable = false;
		// Deixa o jogador invulnerável a qualquer ataque por um breve período de tempo

		/// <summary>
		/// Deine se o jogador está no chão ou não, controlado pelo compnenete GroundDetection
		/// </summary>
		public static bool grounded = false;

		/// <summary>
		/// Deixa o jogador invulnerável por uma certa quantidade de tempo
		/// </summary>
		/// <param name="time">Quantidade de tempo em segundos, em que o jogador permanecerá invulnerável</param>
		public static void SetInvulnerable(float time)
		{
			instance.StopAllCoroutines();
			instance.StartCoroutine(instance.Invulnerability(time));
		}

		/// <summary>
		/// Espera por um certo tempo e deixa o jogador vulnerável novamente
		/// </summary>
		/// <param name="time">quantidade de tempo antes esperada antes de deixar o jogador vulnerável novamente</param>
		/// <returns></returns>
		private IEnumerator Invulnerability(float time)
		{
			invulnerable = true;
			yield return new WaitForSeconds(time);
			invulnerable = false;
		}
		#endregion

#if UNITY_EDITOR
		private bool slow;
#endif

		void Start()
		{
			if (instance == null)
			{
				rb = GetComponent<Rigidbody2D>();
				animator = GetComponent<Animator>();
				boxCollider = GetComponent<BoxCollider2D>();
				circleCollider = GetComponent<CircleCollider2D>();
				startGravScale = rb.gravityScale;

				#region State Machine
				stateMachine = GetComponent<StateMachine>();

				BaseState moveState = GetComponent<MovementState>();
				FreeState freeState = GetComponent<FreeState>();
				DownDashState downDashState = GetComponent<DownDashState>();
				SlidingState slidingState = GetComponent<SlidingState>();
				stateMachine.RegisterState("Move", moveState);
				stateMachine.RegisterState("Free", freeState);
				stateMachine.RegisterState("DownDash", downDashState);
				stateMachine.RegisterState("Sliding", slidingState);
				stateMachine.ChangeState("Move");
				#endregion

				instance = this;
			}
			else
			{
				Debug.LogError($"Mais de uma instância de PlayerCore encontrada, deletando {gameObject.name}");
				Destroy(gameObject);
				return;
			}

			invulnerable = false;
			StopAllCoroutines();
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F1))
			{
				slow = !slow;
				Time.timeScale = slow ? .01f : 1f;
			}
			else if (Input.GetKeyDown(KeyCode.F2))
			{
				Application.targetFrameRate = slow ? 1 : -1;
			}
		}
#endif
	}
}
