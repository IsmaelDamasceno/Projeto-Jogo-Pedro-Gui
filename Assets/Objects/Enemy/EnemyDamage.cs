using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Implementa a interface IAttackble para que o inimigo sofra dano
/// </summary>
public class EnemyDamage : MonoBehaviour, IAttackable
{

	[SerializeField] private GameObject damageParticles;
    private StateMachine machine;
    private Rigidbody2D rb;
	private bool invulnerable = false;

	private AudioSource source;
	private SingleSoundAsset damageSoundAsset;

	/// <summary>
	/// Função a ser executada para o inimigo sofre dano
	/// </summary>
	/// <param name="damage">Dano a ser causado ao inimigo</param>
	/// <param name="attackTransform">Referência ao transform que causou o dano</param>
	/// <param name="direction">Vector representando a direção para jogar o inimigo ao sofrer o dano</param>
	/// <param name="force">Força com que o inimigo será jogado</param>
	/// <param name="torqueIntensity">Intensidade da força a ser usada no torque</param>
    public void SufferDamage(int damage, Transform attackTransform = null, Vector2 direction = default, float force = 1, float torqueIntensity = 1)
    {
		if (invulnerable)
		{
			return;
		}

		if (machine.currentState == "Move")
		{
			machine.ChangeState("Free");
		}

		damageSoundAsset.Play(source);

		Vector2 useDirection = direction;
		if (useDirection == Vector2.zero && attackTransform != default)
		{
			useDirection = (transform.position - attackTransform.position).normalized;
		}
		Vector2 velocity = useDirection * force;

		float torqueDirection = direction.x != 0 ? -Mathf.Sign(direction.x) : Mathf.Sign(Random.Range(-1, 1));
		rb.AddTorque(torqueDirection * force * torqueIntensity, ForceMode2D.Impulse);
		rb.velocity = velocity;

		GetComponent<IFlash>().Flash();
		GetComponent<EnemyFreeState>().StartTimer();
		StopAllCoroutines();
		StartCoroutine(InvulnerabilityCoroutine());
		invulnerable = true;

		DamageParticles particles = Instantiate(damageParticles, transform.position, Quaternion.identity, transform).GetComponent<DamageParticles>();
		particles.SetColor(Color.red);
	}

    // Start is called before the first frame update
    void Start()
    {
        machine = GetComponent<StateMachine>();
        rb = GetComponent<Rigidbody2D>();
		damageSoundAsset = Resources.Load<SingleSoundAsset>("Sound Assets/Damage Single Sound");
		source = GetComponent<AudioSource>();
	}

	IEnumerator InvulnerabilityCoroutine()
	{
		yield return new WaitForSeconds(.2f);
		invulnerable = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
