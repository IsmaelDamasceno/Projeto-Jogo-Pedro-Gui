using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DownDashState : BaseState
{

    [SerializeField] private float gravityForce;
    [SerializeField] private float initialForce;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private GameObject shockWavePrefab; 

    private float originalGravScale;
    private new BoxCollider2D collider;
    private Rigidbody2D rb;
    private RectangleGroundDetection groundDetection;

    public override void Enter()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            collider = GetComponent<BoxCollider2D>();
            groundDetection = new(transform, 0.8f, groundMask, collider);
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
        if (groundDetection.Check())
        {
            for(int i = 0; i <= 1; i++) {
				GameObject obj = Instantiate(shockWavePrefab, transform.position, Quaternion.identity);
                obj.GetComponent<ShockWave>().direction = (int)(Mathf.Sign(i - 1f));
            }
            CameraMovement.ShakeIt(3f, 0.05f);
            machine.ChangeState("Move");
        }
    }

#if UNITY_EDITOR 
    private void OnDrawGizmosSelected()
    {
        if (collider == null)
        {
            return;
        }

        groundDetection.DebugDraw(Color.blue);
    }
#endif
}
