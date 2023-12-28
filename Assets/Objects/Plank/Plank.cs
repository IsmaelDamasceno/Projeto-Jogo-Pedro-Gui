using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Plank : MonoBehaviour
{

    [SerializeField] private GameObject plankPiece;
	[SerializeField] private GameObject dustParticles;
	[SerializeField] private Vector2 torque; 
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
			if (collision.relativeVelocity.magnitude >= 16.5f)
			{

				Vector2 rightDirection = new(0f, 0f);
				float angle = Mathf.Abs(transform.rotation.eulerAngles.z);
				float range = 15f;
				if ((angle > 0f - range && angle < 0f + range) || (angle > 180f - range && angle < 180f + range))
				{
					rightDirection = new(0f, -Math.Sign(collision.relativeVelocity.normalized.x));
				}
				else if
				((angle > 90f - range && angle < 90f + range) || (angle > 270f - range && angle < 270f + range))
				{
					rightDirection = new(Math.Sign(collision.relativeVelocity.normalized.y), 0f);
				}

				foreach(Transform trs in transform)
				{
					Rigidbody2D rb =
						Instantiate(plankPiece, trs.position, Quaternion.identity).GetComponent<Rigidbody2D>();
					Transform dustTrs =
						Instantiate(dustParticles, trs.position, dustParticles.transform.rotation).transform;
					dustTrs.right = rightDirection;

					float magnetude = Mathf.Abs(collision.relativeVelocity.x) /2f;
					Vector2 direction = (trs.position - collision.transform.position).normalized;
					Debug.Log(direction);

					rb.AddForce(direction * magnetude, ForceMode2D.Impulse);
					rb.AddTorque(Math.Sign(magnetude) * Random.Range(torque.x, torque.y), ForceMode2D.Impulse);
				}
				Destroy(gameObject);
			} 
		}
	}
}
