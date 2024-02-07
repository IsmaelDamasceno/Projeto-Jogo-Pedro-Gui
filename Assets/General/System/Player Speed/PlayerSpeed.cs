using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Componente que mostra na tela a velocidade do jogador
/// </summary>
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
            Debug.LogError($"Duas inst�ncias de PlayerSpeed encontradas, deletando {gameObject.name}");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        text.text = $"{Math.Abs(playerRb.velocity.x):00.00} km/h";
    }
}
