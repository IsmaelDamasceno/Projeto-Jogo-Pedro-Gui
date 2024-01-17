using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface implementada a todos os objetos que sofrem dano
/// </summary>
public interface IAttackable
{
    public void SufferDamage(int damage, Transform attackTransform = default, Vector2 direction = default, float force = 1f, float torqueIntensity = 1f);
}
