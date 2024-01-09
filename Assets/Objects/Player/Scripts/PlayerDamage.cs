using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDamage : MonoBehaviour, IAttackable
{

	private Rigidbody2D rb;
	private StateMachine machine;

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

		if (machine.currentState == "Move")
		{
			machine.ChangeState("Free");
		}

		Vector2 useDirection = direction;
		if (useDirection == Vector2.zero && attackTransform != default)
		{
			useDirection = (transform.position - attackTransform.position).normalized;
		}
		Vector2 velocity = useDirection * force;

		rb.AddTorque(-Math.Sign(direction.x) * force * torqueIntensity, ForceMode2D.Impulse);
		rb.velocity = velocity;

		GetComponent<DamageFlash>().Flash();
	}

    void Start()
    {
		machine = GetComponent<StateMachine>();
		rb = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        
    }
}
