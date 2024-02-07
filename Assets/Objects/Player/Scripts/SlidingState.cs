using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingState : BaseState
{

    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTime;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float forwardDistance;
    [SerializeField] private float rayDistance;

    private int slideDirection;

    private IEnumerator ExitCoroutine()
    {
        yield return new WaitForSeconds(slideTime);
        EndState();
	}

    private void EndState()
    {
		machine.ChangeState("Move");
	}

    public override void Enter()
    {
        slideDirection = Math.Sign(transform.localScale.x);
        PlayerCore.rb.gravityScale = 0f;
        PlayerCore.animator.SetBool("Sliding", true);
        PlayerCore.invulnerable = true;

		StopAllCoroutines();
        StartCoroutine(ExitCoroutine());
    }

    public override void Exit()
    {
        PlayerCore.rb.gravityScale = PlayerCore.startGravScale;
		PlayerCore.animator.SetBool("Sliding", false);
		PlayerCore.invulnerable = false;
	}

    public override void FixedStep()
    {
        PlayerCore.rb.velocity = new(slideSpeed * slideDirection, 0f);
    }

    public override void Init()
    {

    }

    public override void Step()
    {
        bool groundRay = Physics2D.Raycast(
            (Vector2)transform.position + (slideDirection * forwardDistance * Vector2.right),
            Vector2.down, rayDistance, groundMask);

		if (!groundRay)
        {
            EndState();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector2 pos0 =
            (Vector2)transform.position + (slideDirection * forwardDistance * Vector2.right);
		Vector2 pos1 =
			(Vector2)transform.position + (slideDirection * forwardDistance * Vector2.right) + (Vector2.down * rayDistance);

		Gizmos.DrawLine(pos0, pos1);
	}
#endif
}
