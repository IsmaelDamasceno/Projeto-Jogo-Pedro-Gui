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

	private CircleGroundDetection groundDetect;

	public override void Enter()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
			collider = GetComponent<CircleCollider2D>();
		}
        rb.freezeRotation = false;

		groundDetect = new(transform, 0.09f, groundMask, collider);

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

		if (groundDetect.Check())
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
		groundDetect.DebugDraw(Color.blue);
	}
#endif
}
