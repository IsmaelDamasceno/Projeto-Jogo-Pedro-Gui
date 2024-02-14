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
        InputListener.pauseEvent.AddListener(Pause);
		pauseObject = GameObject.FindGameObjectWithTag("Pause Menu");
		pauseObject.SetActive(false);
	}

	void Update()
    {
        
    }

    public void Pause()
    {
        paused = !paused;
		bool goingToPause = paused;
		MenuController menuController =
			Utils.SearchObjectWithComponent<MenuController>(pauseObject.transform, "Menu Controller");
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
}
