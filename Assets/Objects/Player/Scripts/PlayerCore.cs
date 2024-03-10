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

		public static MultiSoundAsset slashSound;
		public static MultiSoundAsset grassSteps;
		public static SingleSoundAsset downdashHitSound;
		public static SingleSoundAsset damageSoundEffect;

		public static AudioSource source;
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

		void Awake()
		{
			if (instance == null)
			{
				rb = GetComponent<Rigidbody2D>();
				animator = GetComponent<Animator>();
				boxCollider = GetComponent<BoxCollider2D>();
				circleCollider = GetComponent<CircleCollider2D>();
				startGravScale = rb.gravityScale;
				source = GetComponent<AudioSource>();

				damageSoundEffect = Resources.Load<SingleSoundAsset>("Sound Assets/Damage Single Sound");
				downdashHitSound = Resources.Load<SingleSoundAsset>("Sound Assets/Downdash Hit Single Sound");
				
				slashSound = Resources.Load<MultiSoundAsset>("Sound Assets/Slash Multi Sound");
				slashSound.Setup(this);
				slashSound.SetRepeating(false);

				grassSteps = Resources.Load<MultiSoundAsset>("Sound Assets/Grass Steps Multi Sound");
				grassSteps.Setup(this);
				grassSteps.SetDelay(new Vector2(0.4f, 0.6f));

				#region State Machine
				stateMachine = GetComponent<StateMachine>();

				BaseState moveState = GetComponent<MovementState>();
				FreeState freeState = GetComponent<FreeState>();
				DownDashState downDashState = GetComponent<DownDashState>();
				SlidingState slidingState = GetComponent<SlidingState>();
				NoMoveState noMoveState = GetComponent<NoMoveState>();
				stateMachine.RegisterState("Move", moveState);
				stateMachine.RegisterState("Free", freeState);
				stateMachine.RegisterState("DownDash", downDashState);
				stateMachine.RegisterState("Sliding", slidingState);
				stateMachine.RegisterState("NoMove", noMoveState);
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
