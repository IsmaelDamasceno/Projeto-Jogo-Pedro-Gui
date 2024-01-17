using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe respons�vel por executar congelamentos de tela
/// </summary>
public class TimeFreeze : MonoBehaviour
{
    private static TimeFreeze instance;
    private static float freezeStop;
    private static bool frozen;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
		}
        else
        {
            Destroy(gameObject);
            return;
        }

		Time.timeScale = 1f;
		frozen = false;
	}

    /// <summary>
    /// Fun��o que deve ser executada para congelar a tela
    /// </summary>
    /// <param name="time">Dura��o</param>
    public static void Freeze(float time)
    {
        freezeStop = Time.unscaledTime + time;
        Time.timeScale = 0f;
        frozen = true;
    }

    void Update()
    {
        if (frozen)
        {
            if (Time.unscaledTime > freezeStop)
            {
                Time.timeScale = 1f;
                frozen = false;
            }
        }
    }
}
