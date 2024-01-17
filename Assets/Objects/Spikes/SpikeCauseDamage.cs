using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Executa o dano causado pelo espinho
/// </summary>
public class SpikeCauseDamage : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out IAttackable attackable))
		{
			TimeFreeze.Freeze(0.1f);
			CameraMovement.ShakeIt(2f, 0.1f);
			attackable.SufferDamage(1, transform, Vector2.up, 18f, .1f);
		}
	}
}
