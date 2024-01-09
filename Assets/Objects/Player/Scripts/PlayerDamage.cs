using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour, IAttackable
{
    public void SufferDamage(int damage, Transform attackTransform = null, Vector2 direction = default, float force = 1, float torqueIntensity = 1)
    {
		HealthSystem.ChangeHealth(-damage);
		GetComponent<DamageFlash>().Flash();
	}

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
