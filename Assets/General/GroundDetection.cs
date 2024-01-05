using System;
using UnityEngine;

public class GroundDetection
{
	public GroundDetection(
		Transform transform, float distance, LayerMask detectionMask, bool useRotation = false)
	{
		this.transform = transform;
		this.distance = distance;
		this.detectionMask = detectionMask;
		this.useRotation = useRotation;
	}

	protected LayerMask detectionMask;
	protected bool useRotation;
	protected float distance;
	protected Transform transform;
}

public class RectangleGroundDetection : GroundDetection
{
	public RectangleGroundDetection(Transform transform, float distance, LayerMask detectionMask, BoxCollider2D collider, bool useRotation = false) : base(transform, distance, detectionMask, useRotation)
	{
		this.collider = collider;
	}

	public bool Check()
	{
		Vector2 pos = (Vector2)transform.position + collider.offset;
		Vector2 size = Utils.Vector2Abs((Vector2)transform.localScale * collider.size) - Vector2.right * 0.08f;
		float angle = transform.rotation.eulerAngles.z;

		return Physics2D.BoxCast(pos, size, useRotation ? angle : 0f, Vector2.down, distance, detectionMask);
	}

#if UNITY_EDITOR
	public void DebugDraw(Color color)
	{
		Vector2 pos = (Vector2)transform.position + collider.offset + Vector2.down * distance;
		Vector2 size = Utils.Vector2Abs((Vector2)transform.localScale * collider.size) - Vector2.right * 0.08f;
		float angle = transform.rotation.eulerAngles.z;

		Gizmos.color = color;
		Gizmos.DrawWireCube(pos, size);
	}
#endif

	private BoxCollider2D collider;
}

public class CircleGroundDetection : GroundDetection
{
	public CircleGroundDetection(Transform transform, float distance, LayerMask detectionMask, CircleCollider2D collider) : base(transform, distance, detectionMask, false)
	{
		this.collider = collider;
	}

	public bool Check()
	{
		Vector2 pos = (Vector2)transform.position + collider.offset + Vector2.down * distance;
		float radius = transform.localScale.x * collider.radius - 0.05f;

		return Physics2D.OverlapCircle(pos, radius, detectionMask);
	}

#if UNITY_EDITOR
	public void DebugDraw(Color color)
	{
		Vector2 pos = (Vector2)transform.position + collider.offset + Vector2.down * distance;
		float radius = transform.localScale.x * collider.radius - 0.05f;

		Gizmos.color = color;
		Gizmos.DrawWireSphere(pos, radius);
	}
#endif

	private CircleCollider2D collider;
}
