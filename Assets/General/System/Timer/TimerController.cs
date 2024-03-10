using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [SerializeField] private Color enabledColor = Color.white;
    [SerializeField] private Color disabledColor = Color.black;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float animationTime;

    public static float time;
    public static bool isRunning = false;

    private TextMeshProUGUI textComponent;
    private Image backgroundImage;
    
    private static TimerController instance;

    void Start()
    {
        if (instance == null)
        {
			textComponent = GetComponent<TextMeshProUGUI>();
            backgroundImage = transform.parent.GetComponent<Image>();
            backgroundImage.color = isRunning? enabledColor: disabledColor;
			textComponent.text = ConvertSecondsToMMSSHH(time);

			instance = this;
		}
        else
        {
            Debug.LogError($"Cópia de TimerController encontrada, deletando: {gameObject.name}");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!isRunning)
        {
            return;
        }

        time += Time.deltaTime;
        textComponent.text = ConvertSecondsToMMSSHH(time);
    }

    public static void SetEnabled(bool enabled, float percent = 0f)
    {
        instance.StartCoroutine(instance.SetEnabledCoroutine(enabled, percent));
	}

    IEnumerator SetEnabledCoroutine(bool enabled, float initialPercent)
    {
        int direction = enabled? 1: -1;
        float time;
        if (enabled)
        {
            time = animationTime / initialPercent;
		}
        else
        {
			time = animationTime / (1f - initialPercent);
		}

        while((time <= animationTime && direction == 1) || (time >= 0f && direction == -1))
        {
            time += Time.unscaledDeltaTime * direction;
            time = Mathf.Clamp(time, 0f, animationTime);
            float percent = time / animationTime;
            float point = animationCurve.Evaluate(percent);
            backgroundImage.color = Color.Lerp(disabledColor, enabledColor, point);
			yield return null;
		}
		backgroundImage.color = enabled ? enabledColor : disabledColor;

    }

	public static string ConvertSecondsToMMSSHH(float seconds)
	{
		int minutes = (int)(seconds / 60);
		int remainingSeconds = (int)(seconds % 60);
		int hundredths = (int)((seconds - (int)seconds) * 100);

		return $"{minutes:00}:{remainingSeconds:00}:{hundredths:00}";
	}
}
