using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Executa o dano causado pelo espinho
/// </summary>
public class SpikeCauseDamage : MonoBehaviour
{

	[SerializeField] private float randomAngle;

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out IAttackable attackable))
		{
			float angleOffset = Random.Range(-randomAngle, randomAngle);
			float angleDirection = transform.eulerAngles.z;
			float finalAngle = angleDirection + angleOffset + 90f;
			finalAngle = finalAngle <= 0f ? 360f - (Mathf.Abs(finalAngle)) : finalAngle;
			finalAngle *= Mathf.Deg2Rad;

			Vector2 finalDirection = new(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle));
			attackable.SufferDamage(1, transform, finalDirection, 18f, .1f);
		}
	}
}
