using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDamage : MonoBehaviour, IAttackable
{

    private StateMachine machine;
    private Rigidbody2D rb;

    public void SufferDamage(int damage, int direction = 0)
    {
        Debug.Log($"AI AI {direction}");
        if(machine.currentState == "Move")
        {
			machine.ChangeState("Free");
		}

		Vector2 velocity = new(
			direction * Random.Range(25f, 30f),
			Random.Range(1f, 2f)
		);

		rb.AddTorque(-direction * Random.Range(0.5f, 1f), ForceMode2D.Impulse);
        rb.velocity = velocity;
    }

    // Start is called before the first frame update
    void Start()
    {
        machine = GetComponent<StateMachine>();
        rb = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
