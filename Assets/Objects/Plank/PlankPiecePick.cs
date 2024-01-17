using UnityEngine;

/// <summary>
/// Estado presente quando o pedaço de madeira está solto no chão
/// </summary>
public class PlankPiecePick : Pickable
{
    [Header("Ground Detection")]
    [SerializeField] private float airDrag;
    [SerializeField] private float groundDrag;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float currentDrag;

    private RectangleGroundDetection groundDetection;
    private BoxCollider2D collider;
    private Rigidbody2D rb;

    public override void Enter()
    {
        base.Enter();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        groundDetection = new(transform, 0.08f, groundMask, collider, true);
    }

	public override void Step()
	{
        base.Step();
        currentDrag = groundDetection.Check()? groundDrag: airDrag;
        rb.drag = currentDrag;
	}

    public override void PickUp()
    {

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
