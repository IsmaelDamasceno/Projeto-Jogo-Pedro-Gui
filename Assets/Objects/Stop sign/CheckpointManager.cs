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
        int i = 0;
        foreach(Transform childTrs in transform)
        {
            points.Add(childTrs.position.x);
			if (childTrs.TryGetComponent(out FlagCheckpoint checkpoint))
            {
                checkpoint.index = i;
            }
            i++;
		}
	}

    void Update()
    {
        
    }

    private static void GoToPoint(int point)
    {
        if (point >= points.Count)
        {
            Debug.LogError($"Cannot get point {point}, out of bounds");
            return;
        }
        if (point <= curIndPoint)
        {
			Debug.LogError($"curIndPoint ({curIndPoint}) must be less than point ({point})");
			return;
        }
		minPoint = points[curIndPoint];
        maxPoint = points[point];
        curIndPoint = point;
    }

    public static void StartTrackPlacement(int rightBoundIndex)
    {
        CheckpointSave.activeCheckpoint = (byte)rightBoundIndex;
		GoToPoint(rightBoundIndex);
		TrackReader.LoadPoints((int)Mathf.Floor(minPoint), (int)Mathf.Floor(maxPoint));
    }
    public static void TrackInstaPlacement()
    {
		TrackReader.LoadInstaPoints((int)Mathf.Floor(points[0]), (int)Mathf.Floor(maxPoint));
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
