using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public void SufferDamage(int damage, Transform attackTransform = default, Vector2 direction = default, float force = 1f, float torqueIntensity = 1f);
}
