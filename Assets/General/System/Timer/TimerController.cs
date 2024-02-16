using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public static float time;

    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        time += Time.deltaTime;
        textComponent.text = ConvertSecondsToMMSSHH(time);
    }

	static string ConvertSecondsToMMSSHH(float seconds)
	{
		int minutes = (int)(seconds / 60);
		int remainingSeconds = (int)(seconds % 60);
		int hundredths = (int)((seconds - (int)seconds) * 100);

		return $"{minutes:00}:{remainingSeconds:00}:{hundredths:00}";
	}
}
