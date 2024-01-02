using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFreeState : BaseState
{

	[SerializeField] private LayerMask groundMask;
	[SerializeField] private float getUpTime;

    private Rigidbody2D rb;
	private new CircleCollider2D collider;
	private bool canGetUp = false;

	public override void Enter()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
			collider = GetComponent<CircleCollider2D>();
		}
        rb.freezeRotation = false;
		StartCoroutine(GetUpTimer());
    }

    public override void Exit()
    {
		rb.freezeRotation = true;
		rb.AddForce(Vector2.up * 3.5f, ForceMode2D.Impulse);
		canGetUp = false;
	}

    public override void FixedStep()
    {

    }

    public override void Step()
    {
		if (!canGetUp)
		{
			return;
		}

		if (Grounded())
		{
			machine.ChangeState("Move");
		}
    }

	private bool Grounded()
	{
		Vector2 origin = (Vector2)transform.position + collider.offset + Vector2.down * 0.05f;
		float radius = transform.localScale.x * collider.radius - 0.025f;

		return Physics2D.OverlapCircle(origin, radius, groundMask);
	}

	IEnumerator GetUpTimer()
	{
		yield return new WaitForSeconds(getUpTime);
		canGetUp = true;
	}

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		if (collider == null)
		{
			return;
		}

		Vector2 origin = (Vector2)transform.position + collider.offset + Vector2.down * 0.05f;
		float radius = transform.localScale.x * collider.radius - 0.025f;

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(origin, radius);
	}
#endif
}
