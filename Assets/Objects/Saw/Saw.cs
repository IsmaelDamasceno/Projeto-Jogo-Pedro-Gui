using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Saw : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private AnimationCurve motionCurve;

    private float totalDistance;
    private float currentDitance;
    private int goIndex = 0;

    private Vector2 startPosition;

    private LineRenderer lineRenderer;
    private Vector3[] positions;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        int positionCount = lineRenderer.positionCount;
		positions = new Vector3[positionCount];
        lineRenderer.GetPositions(positions);
        Destroy(lineRenderer);
    }

    void Update()
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
}
