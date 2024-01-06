using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public void SufferDamage(int damage, int direction = 0);
}
