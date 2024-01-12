using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{

    [SerializeField] private int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IAttackable attackable))
        {
			TimeFreeze.Freeze(0.1f);
            CameraMovement.ShakeIt(2f, 0.1f);
			attackable.SufferDamage(damage, transform, Math.Sign(transform.parent.localScale.x) * Vector2.right, 18f, .1f);
        }
    }
}
