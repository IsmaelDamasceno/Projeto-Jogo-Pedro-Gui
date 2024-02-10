using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{

    private static List<float> points = new();

    private static int curIndPoint = 0;

    private static float minPoint = 0;
    private static float maxPoint = 1;

    void Start()
    {
        foreach(Transform childTrs in transform)
        {
            points.Add(childTrs.position.x);
        }

        SetPoint(0);
	}

    void Update()
    {
        
    }

    private static void SetPoint(int point)
    {
        if (point > points.Count - 2)
        {
            Debug.LogError($"Cannot get point {point}, out of bounds");
            return;
        }
		minPoint = points[point];
        maxPoint = points[point + 1];
    }
    private static void GotoNextPoint()
    {
        curIndPoint++;
        SetPoint(curIndPoint);
    }

    public static void StartTrackPlacement()
    {
		TrackReader.LoadPoints((int)Mathf.Floor(minPoint), (int)Mathf.Floor(maxPoint));
		GotoNextPoint();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach(float point in points)
        {
			Gizmos.color = Color.red;

			if (point == minPoint || point == maxPoint)
            {
                Gizmos.color = Color.blue;
            }

            Vector3 pos = new(point, 0f, 0f);
            Gizmos.DrawSphere(pos, 1f);
        }
    }
#endif
}
