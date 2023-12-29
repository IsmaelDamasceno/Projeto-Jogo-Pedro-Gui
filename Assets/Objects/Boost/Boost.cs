using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{

    [SerializeField] private float boostSpeed;

    private SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

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
