using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{

    [SerializeField] private float boostSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<MovementState>().ApplyBoost(Mathf.Sign(transform.localScale.x) * boostSpeed);

            Destroy(gameObject);
        }
    }
}
