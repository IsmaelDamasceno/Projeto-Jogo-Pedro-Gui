using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSpeed : MonoBehaviour
{

    public static PlayerSpeed instance;
    private static Rigidbody2D playerRb;
    private static TextMeshProUGUI text;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
			playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            text = GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError($"Duas instâncias de PlayerSpeed encontradas, deletando {gameObject.name}");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        text.text = $"{Math.Round(Math.Sign(playerRb.velocity.x) * 3.6f, 2)} km/h";
    }
}
