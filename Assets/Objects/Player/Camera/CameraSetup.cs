using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    [SerializeField] private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        CameraMovement.SetTarget(target);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
