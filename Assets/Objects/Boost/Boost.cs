using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Componente colocado no GameObject que da boost ao jogador
/// </summary>
public class Boost : MonoBehaviour
{
    /// <summary>
    /// Quantidade de boost a ser dada ao jogador na colisão com o objeto
    /// </summary>
    [SerializeField] private float boostSpeed;

    /// <summary>
    /// Sprite renderer desse objeto
    /// </summary>
    private SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Código de colisão
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player.MovementState>().ApplyBoost(
                renderer.flipX? -1: 1, Mathf.Sign(transform.localScale.x) * boostSpeed);

            Destroy(gameObject);
        }
    }
}
