using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controla o Health System (adiciona/subtrai vida, seta a vida máxima, etc)
/// </summary>
public class HealthSystem : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;

    public static int health;
    private static int healthMax;

    public static HealthSystem instance;

    private void Awake()
    {
        if (instance == null)
        {
			instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        int health = Utils.SearchObjectWithComponent<Transform>(transform, "Full Set").childCount;
		SetMaxHealth(health);
		SetHealth(health);
	}

	/// <summary>
	/// Set a quantidade de vida
	/// </summary>
	/// <param name="newAmount">Nova quantidade de vida</param>
	public static void SetHealth(int newAmount)
    {
		health = newAmount;
		HealthUpdate();
	}

    /// <summary>
    /// Aumenta a quantidade de vida por um valor
    /// </summary>
    /// <param name="increase">Valor para aumentar/diminuir da vida atual</param>
	public static void ChangeHealth(int increase)
    {
		health += increase;

        if (DeathTest())
        {
            return;
        }

		HealthUpdate();
	}

    private static bool DeathTest()
    {
		if (health <= 0)
		{
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}
        return health <= 0;
	}

    /// <summary>
    /// Atualiza os Hearts ao mudar o valor da vida
    /// </summary>
	private static void HealthUpdate()
    {
        if (health <= 0)
        {
			// instance.StartCoroutine(instance.RestartCoroutine());
		}

        Transform fullSetTransform =
            Utils.SearchObjectWithComponent<Transform>(instance.transform, "Full Set");
        for(int i = 0; i < healthMax; i++)
        {
            HealthController controller =
                fullSetTransform.GetChild(i).GetComponent<HealthController>();
            controller.ChangeHeart(i < health);
        }
    }
	/*
    IEnumerator RestartCoroutine()
    {
		
		 TransitionController.s_Animator.SetTrigger("Start");

		yield return new WaitForSeconds(TransitionController.s_TransitionTime);

		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
		SetMaxHealth(transform.childCount);
		SetHealth(transform.childCount);
		Player.PropertiesCore.Player.transform.position = GameController.savePos;
		 
    }
    */

	/// <summary>
	/// Setar vida máxima
	/// </summary>
	/// <param name="value">Quantidade de vida máxima</param>
	public static void SetMaxHealth(int value)
    {
        int oldMaxHealth = healthMax;
        healthMax = value;

        if (healthMax == Utils.SearchObjectWithComponent<Transform>(instance.transform, "Full Set").childCount)
        {
            return;
        }

        if (oldMaxHealth < value)
        {
			for (int i = oldMaxHealth; i < value; i++)
            {
                Instantiate(instance.heartPrefab, Utils.SearchObjectWithComponent<Transform>(instance.transform, "Full Set"));
            }
			SetHealth(value);
		}
		else if (oldMaxHealth > value)
        {
			for (int i = value; i < Utils.SearchObjectWithComponent<Transform>(instance.transform, "Full Set").childCount; i++)
			{
                Destroy(Utils.SearchObjectWithComponent<Transform>(instance.transform, "Full Set").GetChild(i).gameObject);
			}
			SetHealth(value);
		}
    }
}
