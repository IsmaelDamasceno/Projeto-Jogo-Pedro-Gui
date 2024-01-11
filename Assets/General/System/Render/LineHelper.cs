using UnityEngine;

public class LineHelper
{
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
            lineRenderer.SetPosition(i, pos);
        }
    }

    public static void DrawSpring(LineRenderer lineRenderer, int steps, float radius, float compression, float length)
    {
		lineRenderer.useWorldSpace = false;
		lineRenderer.positionCount = steps;

		for (int i = 0; i < steps; i++)
		{
			float progress = (float)i / steps;
			float angle = (progress * compression) % (2 * Mathf.PI);

			float x = radius * Mathf.Sin(angle);
			float y = progress * length;
			float z = radius * Mathf.Cos(angle);

			Vector3 pos = new(x, y, z);
			lineRenderer.SetPosition(i, pos);
		}
	}
}
