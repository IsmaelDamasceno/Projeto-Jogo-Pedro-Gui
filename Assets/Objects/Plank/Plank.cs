using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Plank : MonoBehaviour
{

    [SerializeField] private GameObject plankPieces;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			if (collision.relativeVelocity.x >= 16.5f)
			{
				foreach(Transform trs in transform)
				{
					Rigidbody2D rb =
						Instantiate(plankPieces, trs.position, Quaternion.identity).GetComponent<Rigidbody2D>();

					float magnetude = collision.relativeVelocity.x/2f;
					Vector2 direction = (trs.position - collision.transform.position).normalized;

					rb.AddForce(direction * magnetude, ForceMode2D.Impulse);
					rb.AddTorque(Math.Sign(magnetude) * Random.Range(5, 10), ForceMode2D.Impulse);
				}
				Destroy(gameObject);
			} 
		}
	}
}
