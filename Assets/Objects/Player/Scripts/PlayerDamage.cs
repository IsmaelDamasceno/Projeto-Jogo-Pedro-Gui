using Player;
using System;
using UnityEngine;

public class PlayerDamage : MonoBehaviour, IAttackable
{
    public void SufferDamage(int damage, Transform attackTransform = null, Vector2 direction = default, float force = 1, float torqueIntensity = 1)
    {
		if (!PlayerCore.ivulnerable)
		{
			PlayerCore.SetIvulnerable(0.1f);
		}
		else
		{
			return;
		}

		HealthSystem.ChangeHealth(-damage);

		if (PlayerCore.stateMachine.currentState == "Move")
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

		GetComponent<DamageFlash>().Flash();
	}

    void Start()
    {
	}

    void Update()
    {
        
    }
}
