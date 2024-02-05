using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{

    private Saw saw;

    void Start()
    {
        saw = GetComponent<Saw>();
    }

    void Update()
    {
        if ((saw.percent >= 1f && saw.direction == 1) || (saw.percent <= 0f && saw.direction == -1))
        {
            saw.direction *= -1;
        }
    }
}
