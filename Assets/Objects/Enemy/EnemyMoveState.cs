using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMoveState : BaseState
{

    [SerializeField] private float rotationTime;

    private Rigidbody2D rb;
    private float currentAngleVel;

    public override void Enter()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
	}

    public override void Exit()
    {

    }

    public override void FixedStep()
    {
        if (Mathf.Abs(transform.rotation.eulerAngles.z) >= 0.1f)
        {
			float angle = transform.rotation.eulerAngles.z;
			transform.rotation =
                Quaternion.Euler(
                    0f, 0f, Mathf.SmoothDampAngle(angle, 0f, ref currentAngleVel, rotationTime));
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }

    public override void Step()
    {

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active)
        {
            return;
        }

        if (collision.CompareTag("ShockWave"))
        {
            machine.ChangeState("Free");

            int hitDirection = Math.Sign(transform.position.x - collision.transform.position.x);
            Vector2 velocity = new(
                hitDirection * Random.Range(2f, 3f),
                Random.Range(12f, 15.5f)
            );

            rb.AddTorque(-hitDirection * Random.Range(0.5f, 1.25f), ForceMode2D.Impulse);
            rb.AddForce(velocity, ForceMode2D.Impulse);
		}
        else if (collision.CompareTag("Player"))
        {
            HealthSystem.ChangeHealth(-1);
        }
    }
}
