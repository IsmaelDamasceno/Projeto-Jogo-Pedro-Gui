using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier;

    private Vector2 currentPos, lastPos, posDifference;
    private new Transform camera;

    void Start()
    {
        camera = Camera.main.transform;
        currentPos = lastPos = camera.position;
        posDifference = Vector2.zero;
    }

    void LateUpdate()
    {
		currentPos = camera.position;
		posDifference = currentPos - lastPos;
        lastPos = currentPos;

        transform.Translate(posDifference * (1f - parallaxMultiplier), Space.Self);
	}
}
