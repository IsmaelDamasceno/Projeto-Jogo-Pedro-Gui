using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Renderiza um circulo usando um line renderer
public class RenderCircle : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        
    }

    public static void DrawCircle(LineRenderer lineRenderer, int steps, float radius)
    {
		lineRenderer.useWorldSpace = false;
		lineRenderer.positionCount = steps;
        for(int i = 0; i < steps; i++)
        {
            float progress = (float)i /steps;
            float angle = progress * 2 * Mathf.PI;

            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);

            Vector3 pos = new(x, y, 0f);
            Debug.Log($"i: {i}, prog: {progress}, ang: {angle}, pos: {pos}");
            lineRenderer.SetPosition(i, pos);
        }
    }
}
