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
    [SerializeField] private AnimationCurve motionCurve;
    [SerializeField] private float bladeRotationSpeed;

    [SerializeField] private List<Chain> chains;

    private float totalDistance;
    private float currentDitance;
    private int goIndex = 0;

    private Vector2 startPosition;

    private LineRenderer lineRenderer;
    private Vector3[] positions;

    private Transform blade;

    void Start()
    {
        blade = transform.GetChild(0);
        lineRenderer = GetComponent<LineRenderer>();

        foreach(Chain chain in chains)
        {
			chain.SetSpeed(speed);
		}

        int positionCount = lineRenderer.positionCount;
		positions = new Vector3[positionCount];
        lineRenderer.GetPositions(positions);
        for(int i = 0; i < positions.Length; i++)
        {
            positions[i] = new(
                positions[i].x * transform.localScale.x, positions[i].y * transform.localScale.y);
            positions[i] += transform.position;
        }
        Destroy(lineRenderer);
    }

    void Update()
    {
		Motion();
        blade.Rotate(new(0f, 0f, 360f * bladeRotationSpeed * Time.deltaTime / 60f), Space.Self);
    }

    private void Motion()
    {
		if (Vector2.Distance(transform.position, positions[goIndex]) <= 0.02f)
		{
			transform.position = (Vector2)positions[goIndex];

			startPosition = positions[goIndex];
			if (goIndex < positions.Length - 1)
			{
				goIndex++;
			}
			else
			{
				goIndex = 0;
			}
			totalDistance = Vector2.Distance(transform.position, positions[goIndex]);
			currentDitance = 0f;
		}
		else
		{
			currentDitance += speed * Time.deltaTime;
			float percent = motionCurve.Evaluate(Mathf.Clamp(currentDitance / totalDistance, 0f, 1f));
			transform.position = Vector2.Lerp(startPosition, positions[goIndex], percent);
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.transform.TryGetComponent(out IAttackable attackable))
		{
			Vector2 direction = (collision.transform.position - transform.position).normalized;
			attackable.SufferDamage(1, default, direction, 12f, .1f);
		}
	}
}
