using Pick;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Pickup : MonoBehaviour
{
    public static Pickup instance;

    [Header("Hold")]
    [SerializeField] private float holdDistance;
    [SerializeField] private float maxWaveAngle;
    [SerializeField] private AnimationCurve waveCurve;
    [SerializeField] private float waveTimeScale;
    [SerializeField] private float oppositePullMiltiplier;

    private float currentWaveTime;
    private float currentAngle;
    private Vector2 currentHoldPosition;

	[Header("Pick Up")]
    [SerializeField] private float pickupRadius;
    [SerializeField] private LayerMask pickupMask;

    public static float PickupRadius { get => instance.pickupRadius; set => instance.pickupRadius = value; }

    private static Collider2D hovering;
	private static Transform holding;
    private Rigidbody2D rb;
    private Transform handsTrs;

	private GameObject armBase;
	private Transform[] armPoints;
	private Transform[] holdPoints;

	private MovementState moveState;

	void Start()
    {
        if (instance == null)
        {
            instance = this;
            rb = GetComponent<Rigidbody2D>();
            moveState = GetComponent<Player.MovementState>();

			armBase = Utils.SearchObjectIntransform(transform, "Arm Points");
			armPoints = Utils.SearchObjectsWithComponent<Transform>(transform, "Arm Points");

			handsTrs = Utils.SearchObjectWithComponent<Transform>(transform, "Hands");
        }
        else
        {
            Debug.LogError($"Duas instâncias de Pickup encontradas, deletando {gameObject.name}");
            Destroy(gameObject);
        }
    }

    private Collider2D GetNearestCollider(Collider2D[] colliderArray)
    {
        Collider2D nearestCollider = null;
        float leastDistance = float.MaxValue;
        foreach(Collider2D collider in colliderArray)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);
			if (distance < leastDistance)
            {
                nearestCollider = collider;
                leastDistance = distance;
			}
        }
        return nearestCollider;
    }
	private void CollisionLogic()
    {
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius, pickupMask);
		if (colliders.Length > 0)
		{
			Collider2D collider = GetNearestCollider(colliders);

			if (hovering != null)
			{
				hovering.GetComponent<Pickable>().hover = false;
			}
			hovering = collider;
			hovering.GetComponent<Pickable>().hover = true;
		}
		else
		{
			if (hovering != null)
			{
				hovering.GetComponent<Pickable>().hover = false;
				hovering = null;
			}
		}
	}
	private void Interact()
	{
		hovering.GetComponent<Pickable>().hover = false;
		holding = hovering.transform;
		hovering = null;

		holding.SetParent(handsTrs);
		holding.GetComponent<PickableCore>().stateMachine.ChangeState("Held");

		holdPoints = new Transform[2];
		int i = 0;
		foreach(Transform trs in holding)
		{
			holdPoints[i] = trs;
			i++;
		}

		armBase.transform.localScale = new(moveState.direction, 1f, 1f);
		handsTrs.localScale = new(moveState.direction, 1f, 1f);
	}
	private void HandAnimation()
    {
		if (Mathf.Abs(rb.velocity.x) >= 2f)
		{
			float oppositeForce =
				Math.Sign(currentWaveTime) != Math.Sign(moveState.direction) ? oppositePullMiltiplier : 1f;
			currentWaveTime += moveState.direction * Time.deltaTime * waveTimeScale * oppositeForce;
			currentWaveTime = Mathf.Clamp(currentWaveTime, -1f, 1f);

			currentAngle =
				Math.Sign(currentWaveTime) * waveCurve.Evaluate(Mathf.Abs(currentWaveTime)) * maxWaveAngle;
			float x = holdDistance * Mathf.Cos((currentAngle + 90f) * Mathf.Deg2Rad);
			float y = holdDistance * Mathf.Sin((currentAngle + 90f) * Mathf.Deg2Rad);

			currentHoldPosition = new(x, y);
		}
		else
		{
			if (Mathf.Abs(currentWaveTime) >= 0.05f)
			{
				currentWaveTime *= .9f;
			}
			else
			{
				currentWaveTime = 0f;
			}
			currentAngle =
				Math.Sign(currentWaveTime) * waveCurve.Evaluate(Mathf.Abs(currentWaveTime)) * maxWaveAngle;
			float x = holdDistance * Mathf.Cos((currentAngle + 90f) * Mathf.Deg2Rad);
			float y = holdDistance * Mathf.Sin((currentAngle + 90f) * Mathf.Deg2Rad);

			currentHoldPosition = new(x, y);
		}
		handsTrs.localPosition = currentHoldPosition;
		handsTrs.rotation = Quaternion.Euler(0f, 0f, currentAngle);
	}
	private void ArmsAnimation()
	{
		if (moveState.moving)
		{
			armBase.transform.localScale = new(moveState.direction, 1f, 1f);
			handsTrs.localScale = new(moveState.direction, 1f, 1f);
		}

		int i = 0;
		foreach(Transform arm in armPoints)
		{
			Transform holdPoint = holdPoints[i];
			Vector2 pos = holdPoint.position - arm.position;
			float distance = pos.magnitude;
			float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - 90f;

			arm.rotation = Quaternion.Euler(0f, 0f, angle);
			i++;
		}
	}

    void Update()
    {
        CollisionLogic();
		if (hovering != null && InputController.GetKeyDown("Pickup"))
		{
			Interact();
		}
		if (holding != null)
		{
			if (!armBase.activeInHierarchy)
			{
				handsTrs.gameObject.SetActive(true);
				armBase.SetActive(true);
			}

			HandAnimation();
			ArmsAnimation();
		}
		else
		{
			if (armBase.activeInHierarchy)
			{
				handsTrs.gameObject.SetActive(false);
				armBase.SetActive(false);
			}
		}
	}

	/*
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position + (Vector3)currentHoldPosition, 0.25f);
	}
#endif
    */

}
