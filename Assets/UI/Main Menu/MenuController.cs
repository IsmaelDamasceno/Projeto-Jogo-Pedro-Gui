using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    [SerializeField] private List<GameObject> panels;
    [SerializeField] private bool isCursorBounded;

    [SerializeField] private bool useKeyboard = true;
    [SerializeField] private bool useMouse = true;

	private List<ToggleButton> options = new();
    private int selectedOption = 0;
	private int lastDirection = 0;

    void Start()
    {
		// Coloca as opções de seleção dentro da lista
		#region Options
		int i = 0;
		Transform optionsTrs = Utils.SearchObjectWithComponent<Transform>(transform, "Options");
		foreach (Transform optionTrs in optionsTrs)
		{
			ToggleButton option = optionTrs.GetComponent<ToggleButton>();
			options.Add(option);
			option.id = i;
			i++;
		}
		options[selectedOption].SetSelected(true);
		#endregion

		InputListener.navigateEvent.AddListener(NavigateListener);
		InputListener.confirmEvent.AddListener(ConfirmListener);
    }

	private void NavigateListener(Vector2 value)
	{
		if (!enabled)
		{
			return;
		}

		int menuDirection = -Math.Sign(value.y);
		if (menuDirection != 0 && menuDirection != lastDirection)
		{
			CursorController.SetCursor(CursorSprite.Hidden);
			if (selectedOption != -1)
			{
				options[selectedOption].SetSelected(false);
			}

			selectedOption += menuDirection;
			if (selectedOption < 0)
			{
				selectedOption = options.Count - 1;
			}
			else if (selectedOption >= options.Count)
			{
				selectedOption = 0;
			}

			options[selectedOption].SetSelected(true);
		}
		lastDirection = menuDirection;
	}
	private void ConfirmListener()
	{
		if (selectedOption != -1 && enabled)
		{
			options[selectedOption].Interact();
		}
	}

    void Update()
    {
	}

    public void ForceSelect(int id)
    {
		if (!enabled || !useMouse)
		{
			return;
		}

        if (isCursorBounded)
        {
			if (selectedOption != -1)
            {
				options[selectedOption].SetSelected(false);
			}
			selectedOption = id;
			if (id != -1)
            {
				options[selectedOption].SetSelected(true);
			}
		}
        else if (id != -1) {
			options[selectedOption].SetSelected(false);
			selectedOption = id;
			options[selectedOption].SetSelected(true);
		}
	}

    public void OpenPanel(string panelName)
    {
		foreach(GameObject panel in panels)
        {
            if (panel.name == panelName)
            {
                panel.SetActive(true);
				enabled = false;
            }
            else
            {
                panel.SetActive(false);
				if (panel.TryGetComponent(out MenuController panelMC))
				{
					panelMC.ForceSelect(-1);
				}
				enabled = true;
			}
        }
	}
    public void CloseActivePanel()
    {
		foreach (GameObject panel in panels)
		{
			if (panel.activeInHierarchy)
			{
				panel.SetActive(false);
				if (panel.TryGetComponent(out MenuController panelMC))
				{
					panelMC.ForceSelect(-1);
				}
				enabled = true;
                return;
			}
		}
        Debug.LogError("Nenhum Painel aberto foi encontrado para fechar");
	}

    public void Play()
    {
		OpenPanel("Play Panel");
	}

    public void StartGame()
    {
		VolumeController.SetProfile("Level Volume Profile");
		InputListener.SetInputMode("Player");
		SceneManager.LoadScene("Level");
	}

    public void Settings()
    {

    }

    public void HighScores()
    {

    }

    public void Quit()
    {

    }
}
