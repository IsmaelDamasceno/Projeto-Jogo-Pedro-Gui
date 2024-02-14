using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static bool paused = false;
    public static GameObject pauseObject;

    void Start()
    {
        InputListener.pauseEvent.AddListener(PauseUsed);
		InputListener.cancelEvent.AddListener(CancelUsed);

		pauseObject = GameObject.FindGameObjectWithTag("Pause Menu");
		pauseObject.SetActive(false);
	}

	void Update()
    {
        
    }

	public void PauseUsed()
	{
		paused = !paused;
		bool goingToPause = paused;
		MenuController menuController =
			Utils.SearchObjectWithComponent<MenuController>(pauseObject.transform, "Menu Controller");

		if (InputListener.activeDevice == "Keyboard")
		{
			if (goingToPause)
			{
				pauseObject.SetActive(true);
				Time.timeScale = 0f;
				VolumeController.SetProfile("Menu Volume Profile");
				InputListener.SetInputMode("UI");
			}
			else
			{
				if (menuController.GetACtivePanel() != null)
				{
					paused = !paused;
					menuController.CloseActivePanel();
				}
				else
				{
					menuController.CloseActivePanel();
					pauseObject.SetActive(false);
					Time.timeScale = 1f;
					VolumeController.SetProfile("Level Volume Profile");
					InputListener.SetInputMode("Player");
				}
			}
		}
		else
		{
			if (goingToPause)
			{
				pauseObject.SetActive(true);
				Time.timeScale = 0f;
				VolumeController.SetProfile("Menu Volume Profile");
				InputListener.SetInputMode("UI");
			}
			else
			{
				menuController.CloseActivePanel();
				pauseObject.SetActive(false);
				Time.timeScale = 1f;
				VolumeController.SetProfile("Level Volume Profile");
				InputListener.SetInputMode("Player");
			}
		}
	}
	public void CancelUsed()
	{
		MenuController menuController =
			Utils.SearchObjectWithComponent<MenuController>(pauseObject.transform, "Menu Controller");

		if (paused && menuController.GetACtivePanel() != null && InputListener.activeDevice != "Keyboard")
		{
			menuController.CloseActivePanel();
		}
	}
}
