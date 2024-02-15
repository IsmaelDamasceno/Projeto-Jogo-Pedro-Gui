using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

/// <summary>
/// Associado a serra (controla movimento e dano)
/// </summary>
public class Saw : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float bladeRotationSpeed;
    [SerializeField] private float force = 25f;

    private float totalDistance;
    private float currentDitance = 0;
    public int direction = 1;
    public float percent = 0f;

    private LineRenderer lineRenderer;
    private Vector3[] positions;

    private Transform blade;

	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}

	void Start()
    {
        blade = transform.GetChild(0);
        lineRenderer = GetComponent<LineRenderer>();

        int positionCount = lineRenderer.positionCount;
		positions = new Vector3[positionCount];
        lineRenderer.GetPositions(positions);
        for(int i = 0; i < positions.Length; i++)
        {
            positions[i] = new(
                positions[i].x * transform.localScale.x, positions[i].y * transform.localScale.y);
            positions[i] += transform.position;
        }
        totalDistance = Vector2.Distance(positions[0], positions[1]);
        Destroy(lineRenderer);

        int index = direction == 1 ? 1 : 0;
        currentDitance = Vector2.Distance(transform.position, positions[index]);
		percent = Mathf.Clamp(currentDitance / totalDistance, 0f, 1f);
	}

    void Update()
    {
		Motion();
        blade.Rotate(new(0f, 0f, 360f * bladeRotationSpeed * Time.deltaTime / 60f), Space.Self);
    }

    private void Motion()
    {
		if ((currentDitance > 0f && direction == -1) || (currentDitance < totalDistance && direction == 1))
        {
			currentDitance += speed * Time.deltaTime * direction;
		}
		percent = Mathf.Clamp(currentDitance / totalDistance, 0f, 1f);
		transform.position = Vector2.Lerp(positions[0], positions[1], percent);
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
		if (collision.transform.TryGetComponent(out IAttackable attackable))
		{
			Vector2 direction = (collision.transform.position - transform.position).normalized;
			attackable.SufferDamage(1, default, direction, force, .1f);
		}
	}
}
