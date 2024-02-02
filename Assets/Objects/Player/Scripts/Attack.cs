using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla a animação de ataque do jogador
/// </summary>
public class Attack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;

    [HideInInspector] public bool attacking = false;
    private bool canAttack = true;

    private GameObject attackCollision;
    private Animator animator;

    void Start()
    {
        attackCollision = Utils.SearchObjectIntransform(transform, "Attack Collider");
        attackCollision.SetActive(false);

        animator = GetComponent<Animator>();

        InputListener.attackEvent.AddListener(AttackListener);
    }

    public void AttackListener()
    {
        if (canAttack && enabled)
        {
			PerformAttack();
		}
    }

    private void PerformAttack()
    {
        canAttack = false;
        attacking = true;
		attackCollision.SetActive(true);
        animator.SetTrigger("Attack");
	}

    public void EndAttack()
    {
		attacking = false;
        attackCollision.SetActive(false);
		StartCoroutine(WaitCooldown());
	}

    private IEnumerator WaitCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
