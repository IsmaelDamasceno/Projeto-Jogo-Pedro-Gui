using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour, IAttackable
{
    public void SufferDamage(int damage, int direction = 0)
    {
        HealthSystem.ChangeHealth(-damage);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
