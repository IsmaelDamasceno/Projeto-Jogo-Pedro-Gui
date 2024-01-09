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

    private static int health;
    private static int healthMax;
    private static GridLayoutGroup layoutGroup;

    public static HealthSystem instance;

    /// <summary>
    /// Quantidade máxima de corações por linha
    /// </summary>
    /// <param name="newValue">Nova quantidade máxima</param>
    public static void SetHeartPerRow(int newValue)
    {
        layoutGroup.constraintCount = newValue;
    }

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
		layoutGroup = GetComponent<GridLayoutGroup>();
		SetMaxHealth(transform.childCount);
		SetHealth(transform.childCount);
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

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

		HealthUpdate();
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

        for(int i = 0; i < healthMax; i++)
        {
            HealthController controller = instance.transform.GetChild(i).GetComponent<HealthController>();
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

        if (healthMax == instance.transform.childCount)
        {
            return;
        }

        if (oldMaxHealth < value)
        {
			for (int i = oldMaxHealth; i < value; i++)
            {
                Instantiate(instance.heartPrefab, instance.transform);
            }
			SetHealth(value);
		}
		else if (oldMaxHealth > value)
        {
			for (int i = value; i < instance.transform.childCount; i++)
			{
                Destroy(instance.transform.GetChild(i).gameObject);
			}
			SetHealth(value);
		}
    }
}
