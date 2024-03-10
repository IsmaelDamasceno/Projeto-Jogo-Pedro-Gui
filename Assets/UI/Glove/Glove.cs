using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class Glove : MonoBehaviour
{
    [Header("Charge")]
    [SerializeField] private float cooldownTime;

    [Header("Colors")]
    [SerializeField] private AnimationCurve colorBlendCurve;
    [SerializeField] private Color unchargedColor = Color.white;
    [SerializeField] private Color chargedColor = Color.white;

    [Header("Animation")]
    [SerializeField] private AnimationClip chargeClip;

    private static bool charged = true;

    private static RectTransform rectTrs;
    private static Animator animator;

    private static Glove instance;
    private static Image image;

    IEnumerator ChargeWaitCoroutine()
    {
        yield return new WaitForSeconds(cooldownTime);
		charged = true;
	}
	IEnumerator ChargeVisualCoroutine()
    {
		float time = 0f;
		float totalTime = cooldownTime - chargeClip.length;
		if (totalTime <= 0f)
		{
			Debug.LogError($"total glove charge time is negative: {totalTime}");
		}
		while (time < totalTime)
		{
			time += Time.deltaTime;
			time = Mathf.Clamp(time, 0f, cooldownTime);
			float percent = time / totalTime;
			float point = colorBlendCurve.Evaluate(percent);
			image.color = Color.Lerp(unchargedColor, chargedColor, point);

			yield return null;
		}
		image.color = chargedColor;
		animator.SetTrigger("Charge");
	}

    public static bool UseAbility()
    {
        if (!charged)
        {
            return false;
        }
        charged = false;
        instance.StartCoroutine(instance.ChargeWaitCoroutine());
		instance.StartCoroutine(instance.ChargeVisualCoroutine());

		return true;
    }

    void Start()
    {
        instance = this;
        rectTrs = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        charged = true;
	}

    void Update()
    {
        
    }
}
