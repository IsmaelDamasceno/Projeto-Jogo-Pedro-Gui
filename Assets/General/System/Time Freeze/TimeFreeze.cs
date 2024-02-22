using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe responsável por executar congelamentos de tela
/// </summary>
public class TimeFreeze : MonoBehaviour
{
    private static TimeFreeze instance;
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
    /// Função que deve ser executada para congelar a tela
    /// </summary>
    /// <param name="time">Duração</param>
    public static void Freeze(float time)
    {
        instance.StartCoroutine(instance.FreezeCoroutine(time));
	}
    private IEnumerator FreezeCoroutine(float time)
    {
        Time.timeScale = 0f;
        frozen = true;
        yield return new WaitForSecondsRealtime(time);
        frozen = false;
        if (!PauseController.paused)
        {
			Time.timeScale = 1f;
		}
    }
}
