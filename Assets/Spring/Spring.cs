using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

    [SerializeField] private float force;

    // Componente Animator
    private Animator animator;

    void Start()
    {
        // Procura o componente animator no Game Object
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Rigidbody2D rb))
        {
            rb.velocity = Vector2.up * force;

			animator.Play("Base Layer.Idle");
			animator.SetTrigger("Activate");
        }
    }
}
