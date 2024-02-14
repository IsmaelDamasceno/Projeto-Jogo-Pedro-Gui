using Player;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class FlagCheckpoint : MonoBehaviour
{

    [SerializeField] private float animationTime;
    [SerializeField] private AnimationCurve animationCurve;
	[SerializeField] private float initialDelay;

	private GameObject flag;
    public int index;
    private float startScale;
    private Color startColor;

    private SpriteRenderer sprRenderer;

    void Start()
    {
        flag = transform.GetChild(0).gameObject;
        flag.transform.localScale = new(0f, 1f, 1f);
        flag.SetActive(false);

        startScale = transform.localScale.x;

        sprRenderer = GetComponent<SpriteRenderer>();
        startColor = sprRenderer.color;

		if (CheckpointSave.activeCheckpoint >= index)
		{
			Play();
		}
		if (CheckpointSave.activeCheckpoint == index)
		{
			PlayerCore.rb.transform.position = transform.position;
		}
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerCore.stateMachine.currentState == "Move" && !flag.activeInHierarchy)
        {
            CheckpointManager.StartTrackPlacement(index, this);
		}
    }

    public void Play()
    {
		flag.SetActive(true);
		StopAllCoroutines();
		StartCoroutine(FlagAnimation());
	}

    private IEnumerator FlagAnimation()
    {
        yield return new WaitForSeconds(initialDelay);

        float time = 0f;
        Color finalColor = Color.white;
        while (time < animationTime)
        {
            time += Time.deltaTime;
            float percent = Mathf.Clamp(time / animationTime, 0f, 1f);
            float point = animationCurve.Evaluate(percent);
			flag.transform.localScale = new(point, 1f, 1f);
            sprRenderer.color = Color.Lerp(startColor, finalColor, point);
            transform.localScale = new(Mathf.Lerp(startScale, 1f, point), 1f, 1f);

            yield return null;
        }
		flag.transform.localScale = new(1f, 1f, 1f);
	}
}
