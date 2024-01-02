using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpring : MonoBehaviour
{

    [SerializeField] private int steps;
	[SerializeField] private float radius;
	[SerializeField] private float compression;
	[SerializeField] private float length;

	private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        LineHelper.DrawSpring(lineRenderer, steps, radius, compression, length);
    }
}
