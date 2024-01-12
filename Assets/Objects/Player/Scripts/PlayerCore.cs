using System.Collections;
using UnityEngine;

namespace Player
{
	public class PlayerCore : MonoBehaviour
	{

		[SerializeField] private LayerMask groundMask;

		public static LayerMask GroundMask { get => instance.groundMask; set => instance.groundMask = value; }

		public static StateMachine stateMachine;
		public static PlayerCore instance;

		public static Rigidbody2D rb;
		public static Animator animator;
		public static BoxCollider2D boxCollider;
		public static CircleCollider2D circleCollider;
		public static RectangleGroundDetection rectGroundDetection;

		public static float startGravScale;
		// public static CircleGroundDetection circGroundDetection;

		#region Ivulnerability
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
			Time.timeScale = slow ? .1f : 1f;
		}
#endif
	}
}
