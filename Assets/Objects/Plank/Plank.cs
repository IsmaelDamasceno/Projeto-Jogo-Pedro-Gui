using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

	public void Explode(Vector2 speed, Vector2 point )
	{

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			float maxSpeed = collision.transform.GetComponent<Player.MovementState>().GetMaxRegularSpeed();

			Vector2 hitDirection = collision.contacts[0].normal;
			Vector2 velocityTowardsPlank = collision.relativeVelocity * hitDirection;
			Debug.Log(velocityTowardsPlank);

			if (velocityTowardsPlank.magnitude >= maxSpeed - 1f)
			{
				foreach(Transform trs in transform)
				{
					#region Plank Piece
					Rigidbody2D plankPieceRb =
						Instantiate(plankPiece, trs.position, Quaternion.identity).GetComponent<Rigidbody2D>();

					float magnetude = Mathf.Clamp(velocityTowardsPlank.magnitude / 4f, 0f, 12f);
					Vector2 direction = (trs.position - collision.transform.position).normalized;

					plankPieceRb.AddForce(direction * magnetude, ForceMode2D.Impulse);
					plankPieceRb.AddTorque(Math.Sign(magnetude) * Random.Range(torque.x, torque.y), ForceMode2D.Impulse);
					#endregion

					#region Dust
					Transform dustTrs =
						Instantiate(dustParticles, trs.position, dustParticles.transform.rotation).transform;
					dustTrs.right = hitDirection;
					#endregion
				}
				Destroy(gameObject);
			} 
		}
	}
}
