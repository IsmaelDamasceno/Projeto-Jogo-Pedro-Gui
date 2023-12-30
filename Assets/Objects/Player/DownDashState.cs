using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownDashState : BaseState
{

    [SerializeField] private float gravityForce;
    [SerializeField] private float initialForce;
    [SerializeField] private LayerMask groundMask;

    private float originalGravScale;
    private new BoxCollider2D collider;
    private Rigidbody2D rb;

    public override void Enter()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            collider = GetComponent<BoxCollider2D>();
		}

        originalGravScale = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.down * initialForce;
    }

    public override void Exit()
    {
        rb.gravityScale = originalGravScale;
    }

    public override void FixedStep()
    {

    }

    public override void Step()
    {
        if (Grounded())
        {
            machine.ChangeState("Move");
        }
    }

    private bool Grounded()
    {
		Vector2 origin = (Vector2)transform.position + collider.offset + Vector2.down * 0.01f;
		Vector2 size = ((Vector2)transform.localScale - Vector2.right * 0.05f) * collider.size;
		return Physics2D.BoxCast(origin, size, 0f, Vector2.down, 0f, groundMask);
	}

#if UNITY_EDITOR 
    private void OnDrawGizmosSelected()
    {
        if (collider == null)
        {
            return;
        }

		Vector2 origin = (Vector2)transform.position + collider.offset + Vector2.down * 0.01f;
		Vector2 size = ((Vector2)transform.localScale - Vector2.right * 0.05f) * collider.size;

		Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(origin, size);
    }
#endif
}
