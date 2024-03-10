using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float cooldownTime;
    [SerializeField] private Vector2 force;

    private Animator animator;
    private Transform shootPivotTrs;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        shootPivotTrs = transform.GetChild(1);

		StartCoroutine(ShootCoroutine());
	}

    void Update()
    {
        
    }

    IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(cooldownTime);
		Rigidbody2D bombRb = 
            Instantiate(bombPrefab, shootPivotTrs.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        bombRb.AddForce(new Vector2(Random.Range(force.x, force.y) * -System.Math.Sign(transform.localScale.x), 1f), ForceMode2D.Impulse);

        animator.SetTrigger("Shoot");

        StartCoroutine(ShootCoroutine());
    }
}
