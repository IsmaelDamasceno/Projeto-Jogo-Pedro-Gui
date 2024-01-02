using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShockWave : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float time;
    [SerializeField] private LayerMask groundMask;

    private new BoxCollider2D collider;
    private bool willDestroy = false;

    public int direction;
    void Start()
    {
        StartCoroutine(DestroyTime());
        transform.localScale = new(direction, 1f, 1f);
        collider = GetComponent<BoxCollider2D>();
	}

    private void Update()
    {
        if (WallDetection())
        {
			Destroy(gameObject);
		}
	}

    void FixedUpdate()
    {
        transform.Translate(direction * speed * Vector2.right, Space.Self);
    }

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }

    private bool WallDetection()
    {
		Vector2 origin = (Vector2)transform.position + collider.offset + (direction * speed * Vector2.right * 0.1f);
		Vector2 size = ((Vector2)transform.localScale - Vector2.up * 0.05f) * collider.size;
        
        return Physics2D.OverlapBox(origin, size, 0f, groundMask);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		if (collider == null)
		{
			return;
		}

		Vector2 origin = (Vector2)transform.position + collider.offset + (direction * speed * Vector2.right * 0.1f);
		Vector2 size = ((Vector2)transform.localScale - Vector2.up * 0.05f) * collider.size;

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(origin, size);
	}
#endif
}
