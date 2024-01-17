using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputAxis
{
    public InputAxis(string negative, string positive)
    {
        this.negative = negative;
        this.positive = positive;

        val = 0f;
    }

    public int GetValRaw()
    {
        int negative = Convert.ToInt32(Input.GetKey(InputController.keys[this.negative]));
		int positive = Convert.ToInt32(Input.GetKey(InputController.keys[this.positive]));

        return positive - negative;
	}

    private string negative;
    private string positive;
    public float val;
}

/// <summary>
/// Detecta inputs de teclado (classe provísória e bosta)
/// </summary>
public class InputController : MonoBehaviour
{
    public static Dictionary<string, KeyCode> keys;
    public static InputAxis moveAxis;
    public static InputAxis verticalAxis;

    private static InputController instance;

    void Start()
    {
        if (instance == null)
        {
			keys = new()
		    {
			    { "Left", KeyCode.A },
			    { "Right", KeyCode.D },
                { "Up", KeyCode.W },
                { "Down", KeyCode.S },
                { "Jump", KeyCode.Space },
                { "Attack", KeyCode.J },
                { "DownDash", KeyCode.S }
            };
			moveAxis = new("Left", "Right");
            verticalAxis = new("Up", "Down");

			DontDestroyOnLoad(gameObject);
		}
        else
        {
            Debug.LogError($"Mais de uma instância do script InputController encontrada, destruindo game object {gameObject.name}");
            Destroy(gameObject);
        }
    }

    public static bool GetKey(string key)
    {
        return Input.GetKey(keys[key]);
    }
    public static bool GetKeyDown(string key)
    {
        return Input.GetKeyDown(keys[key]);
    }

}
