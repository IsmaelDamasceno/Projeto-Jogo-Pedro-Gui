using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// Compoenente associado ao objeto de shockwave quando o jogador faz um down dash
/// </summary>
public class ShockWave : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float time;
    [SerializeField] private LayerMask groundMask;

    private new BoxCollider2D collider;
    private ParticleSystem partSystem;
    private bool willDestroy = false;

    public int direction;
    void Start()
    {
        StartCoroutine(DestroyTime());
        transform.localScale = new(direction, 1f, 1f);
        collider = GetComponent<BoxCollider2D>();
        partSystem = GetComponent<ParticleSystem>();

        RaycastHit2D hitinfo = Physics2D.Raycast(
            (Vector2)transform.position + collider.offset, Vector2.down, 3f, groundMask);
	    if (hitinfo)
        {
            transform.position += Vector3.down * hitinfo.distance + Vector3.up * 0.5f;
        }
    }

    private void Update()
    {
        if (WallDetection())
        {
            Deactivate();
		}

        if (partSystem.particleCount == 0 && willDestroy)
        {
            Destroy(gameObject);
        }
	}

    void FixedUpdate()
    {
        if (willDestroy)
        {
            return;
        }
        transform.Translate(direction * speed * Vector2.right, Space.Self);
    }

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(time);
        Deactivate();
	}

    private void Deactivate()
    {
		collider.enabled = false;
		willDestroy = true;
		partSystem.Stop();
	}

    private bool WallDetection()
    {
		Vector2 origin = (Vector2)transform.position + collider.offset + (direction * speed * Vector2.right * 0.1f);
		Vector2 size = ((Vector2)transform.localScale - Vector2.up * 0.05f) * collider.size;
        
        return Physics2D.OverlapBox(origin, size, 0f, groundMask);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.TryGetComponent(out IAttackable attackable))
		{
			TimeFreeze.Freeze(0.1f);
			CameraMovement.ShakeIt(2f, 0.1f);

			attackable.SufferDamage(1, default, Vector2.up, 14f, 0.1f);
		}
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
