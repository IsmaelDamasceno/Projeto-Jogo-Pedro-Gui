using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla o Estado e Sprite de Hearts no Health System
/// </summary>
public class HealthController : MonoBehaviour
{
    /// <summary>
    /// Se o Heart está preenchido
    /// </summary>
    public bool full = true;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Muda o estado do Heart
    /// </summary>
    /// <param name="value">Novo estado: false (vazio), ou true (preenchido)</param>
    public void ChangeHeart(bool value)
    {
        full = value;
        if (value == false)
        {
			animator.SetTrigger("Trigger");
		}
    }
}
