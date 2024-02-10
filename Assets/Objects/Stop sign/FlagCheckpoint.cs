using Player;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class FlagCheckpoint : MonoBehaviour
{

    [SerializeField] private float animationTime;
    [SerializeField] private AnimationCurve animationCurve;

    private GameObject flag;

    void Start()
    {
        flag = transform.GetChild(0).gameObject;
        flag.transform.localScale = new(0f, 1f, 1f);
        flag.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerCore.stateMachine.currentState == "Move" && !flag.activeInHierarchy)
        {
            flag.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(FlagAnimation());
        }
    }

    private IEnumerator FlagAnimation()
    {
        float time = 0f;
        while (time < animationTime)
        {
            time += Time.deltaTime;
            float percent = Mathf.Clamp(time / animationTime, 0f, 1f);
            float point = animationCurve.Evaluate(percent);
			flag.transform.localScale = new(point, 1f, 1f);

            yield return null;
        }
		flag.transform.localScale = new(1f, 1f, 1f);
	}
}
