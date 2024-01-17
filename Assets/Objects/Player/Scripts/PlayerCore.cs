using System.Collections;
using UnityEngine;

namespace Player
{
	/// <summary>
	/// Core da máquina de estado do jogador, inclui variáveis globais usadas por diferentes estados para evitar duplicar referências
	/// </summary>
	public class PlayerCore : MonoBehaviour
	{

		[SerializeField] private LayerMask groundMask;

		public static LayerMask GroundMask { get => instance.groundMask; set => instance.groundMask = value; }

		public static StateMachine stateMachine;
		public static PlayerCore instance;

		#region Global Variables
		public static Rigidbody2D rb;
		public static Animator animator;
		public static BoxCollider2D boxCollider;
		public static CircleCollider2D circleCollider;
		public static RectangleGroundDetection rectGroundDetection;
		#endregion

		public static float startGravScale;
		// public static CircleGroundDetection circGroundDetection;

		#region Ivulnerability
		public static bool invulnerable = false;
		// Deixa o jogador invulnerável a qualquer ataque por um breve período de tempo
		public static void SetInvulnerable(float time)
		{
			instance.StopAllCoroutines();
			instance.StartCoroutine(instance.Invulnerability(time));
		}
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
				rectGroundDetection = new(transform, 0.08f, groundMask, boxCollider);
				startGravScale = rb.gravityScale;

				#region State Machine
				stateMachine = GetComponent<StateMachine>();

				BaseState moveState = GetComponent<MovementState>();
				FreeState freeState = GetComponent<FreeState>();
				DownDashState downDashState = GetComponent<DownDashState>();
				stateMachine.RegisterState("Move", moveState);
				stateMachine.RegisterState("Free", freeState);
				stateMachine.RegisterState("DownDash", downDashState);
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
			}
			Time.timeScale = slow ? .1f : 1f;
		}
#endif
	}
}
