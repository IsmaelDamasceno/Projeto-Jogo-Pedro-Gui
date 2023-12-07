using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;

    [SerializeField] private float jumpStrength;

    // Camadas que representam ch�o onde o jogador pode pular
    [SerializeField] private LayerMask groundMask;

    // Compenente que controla a f�sica do Game Object
    private Rigidbody2D rb;

    // Componente que controla colis�es do Game Object
    private BoxCollider2D collider;

    private int input;

    void Start()
    {
		// Procura um Rigidbody2D no Game Object, e atribui seu valor a vari�vel
		rb = GetComponent<Rigidbody2D>();

		// Procura um BoxCollider2D no Game Object, e atribui seu valor a vari�vel
		collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        input = InputController.moveAxis.GetValRaw();
	}

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(input * moveSpeed, rb.velocity.y);

        if (InputController.GetKey("Jump") && Grounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        }
    }

    private bool Grounded()
    {
        return Physics2D.BoxCast(
            transform.position, (new Vector2(collider.size.x - 0.1f, collider.size.y)) * transform.localScale, 0f, Vector2.down, 0.05f, groundMask);
    }
}
