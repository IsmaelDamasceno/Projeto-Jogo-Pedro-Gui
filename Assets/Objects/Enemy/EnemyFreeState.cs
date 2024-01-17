using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Estado presente quando o inimigo está caído no chão após sofrer dano
/// </summary>
public class EnemyFreeState : BaseState
{
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private float getUpTime;

	[SerializeField] private float airDrag;
	[SerializeField] private float groundDrag;

    private Rigidbody2D rb;
	private new CircleCollider2D collider;
	private bool canGetUp = false;

	private CircleGroundDetection groundDetect;

	public override void Enter()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
			collider = GetComponent<CircleCollider2D>();
		}
        rb.freezeRotation = false;

		groundDetect = new(transform, 0.5f, groundMask, collider);

		StartCoroutine(GetUpTimer());
    }

    public override void Exit()
    {
		rb.freezeRotation = true;
		rb.AddForce(Vector2.up * 3.5f, ForceMode2D.Impulse);
		rb.drag = 1f;
		canGetUp = false;
	}

    public override void FixedStep()
    {

    }

    public override void Step()
    {
		bool grounded = groundDetect.Check();
		rb.drag = grounded ? groundDrag : airDrag;

		if (!canGetUp)
		{
			return;
		}

		if (grounded)
		{
			machine.ChangeState("Move");
		}
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

		groundDetect.DebugDraw(Color.white);
	}
#endif
}
