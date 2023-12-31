using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMoveState : BaseState
{

    [SerializeField] private float rotationTime;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Header("Collision Check")]
    [SerializeField] private float wallDistance;
    [SerializeField] private float edgeCheckRadius;
    [SerializeField] private float edgeCheckDistance;
    [SerializeField] private LayerMask collisionMask;

    private Rigidbody2D rb;
    private float currentAngleVel;
    private CircleCollider2D collider;

    private bool moving = true;

    [SerializeField] private int direction = 1;

    public override void Enter()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            collider = GetComponent<CircleCollider2D>();
        }
        else
        {
            moving = false;
        }
	}

    public override void Exit()
    {

    }

	public override void Step()
	{
		#region Movement
		transform.localScale = new(direction, 1f, 1f);
		#endregion
	}

	public override void FixedStep()
    {
        #region Movement
        if (moving)
        {
            if (CheckWall() || !CheckEdge())
            {
                direction *= -1;
            }

		    rb.velocity =
		       direction * moveSpeed * Vector2.right +
		       Vector2.up * rb.velocity.y;
		}
		#endregion

		#region GetUp Rotation
		else
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
                moving = true;
			}
		}
		#endregion
	}


    private bool CheckWall()
    {
        Vector2 pos =
            (Vector2)transform.position + collider.offset;
        return Physics2D.Raycast(pos, Vector2.right * direction, collider.radius + wallDistance, collisionMask);
    }
    private bool CheckEdge()
    {
		Vector2 pos =
			(Vector2)transform.position + collider.offset +
             edgeCheckDistance * direction * Vector2.right +
             Vector2.down * collider.radius;

        return Physics2D.OverlapCircle(pos, edgeCheckRadius, collisionMask);
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
            collision.GetComponent<IAttackable>().SufferDamage(1);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (collider == null)
        {
            return;
        }

		Vector2 raycastPos =
			(Vector2)transform.position + collider.offset;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(raycastPos, raycastPos + Vector2.right * (collider.radius + wallDistance));


		Vector2 edgePos =
			(Vector2)transform.position + collider.offset +
			 edgeCheckDistance * direction * Vector2.right +
			 Vector2.down * collider.radius;

		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(edgePos, edgeCheckRadius);
	}
#endif
}
