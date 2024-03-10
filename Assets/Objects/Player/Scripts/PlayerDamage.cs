using Player;
using System;
using UnityEngine;

public class PlayerDamage : MonoBehaviour, IAttackable
{

	[SerializeField] private GameObject damageParticles;

    public void SufferDamage(int damage, Transform attackTransform = null, Vector2 direction = default, float force = 1, float torqueIntensity = 1)
    {
		if (!PlayerCore.invulnerable)
		{
			PlayerCore.SetInvulnerable(2f);
		}
		else
		{
			return;
		}

		PlayerCore.damageSoundEffect.Play(PlayerCore.source);

		TimeFreeze.Freeze(0.1f);
		CameraMovement.ShakeIt(2f, 0.1f);
		HealthSystem.ChangeHealth(-damage);

		if (PlayerCore.stateMachine.currentState != "Free")
		{
			PlayerCore.stateMachine.ChangeState("Free");
		}

		Vector2 useDirection = direction;
		if (useDirection == Vector2.zero && attackTransform != default)
		{
			useDirection = (transform.position - attackTransform.position).normalized;
		}
		Vector2 velocity = useDirection * force;

		PlayerCore.rb.AddTorque(-Math.Sign(direction.x) * force * torqueIntensity, ForceMode2D.Impulse);
		PlayerCore.rb.velocity = velocity;

		GetComponent<IFlash>().Flash();
		DamageParticles particles = Instantiate(damageParticles, transform.position, Quaternion.identity, transform).GetComponent<DamageParticles>();
		particles.SetColor(Color.white);
	}

    void Start()
    {
	}

    void Update()
    {
        
    }
}
