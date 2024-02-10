using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCheckpoint : MonoBehaviour
{

    private GameObject flag;

    void Start()
    {
        flag = transform.GetChild(0).gameObject;
        flag.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            flag.SetActive(true);
        }
    }
}
