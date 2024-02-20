using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField] private List<Sprite> numbers;

    private Animator animator;
    private Image imageRenderer;

    private static Counter instance;
    private static bool triggeredOnce = false;

    private SingleSoundAsset countSound;
    private SingleSoundAsset releaseSound;
    private void OnEnable()
    {
        if (triggeredOnce)
        {
			StopAllCoroutines();
			StartCoroutine(Count());
			TimerController.isRunning = false;
			TimerController.SetEnabled(true);
            PauseController.allowPause = false;
		}
	}
    private void OnDisable()
    {
		if (triggeredOnce && ClockCollision.clockColected)
        {
			Time.timeScale = 1f;
			TimerController.isRunning = true;
			TimerController.SetEnabled(true, 1f);
			VolumeController.SetProfile("Level Volume Profile");
			PauseController.allowPause = true;
		}

        triggeredOnce = true;
	}

    void Start()
    {
        if (instance == null)
        {
			animator = GetComponent<Animator>();
			imageRenderer = GetComponent<Image>();

			countSound = Resources.Load<SingleSoundAsset>("Sound Assets/Clock Count Single Sound");
			countSound.Setup(GetComponent<AudioSource>());

			releaseSound = Resources.Load<SingleSoundAsset>("Sound Assets/Clock Release Single Sound");
			releaseSound.Setup(GetComponent<AudioSource>());

			instance = this;
            transform.parent.gameObject.SetActive(false);
		}
        else
        {
            Debug.LogError($"duas instâncias de Counter encontradas, deletando {gameObject.name}");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }

    IEnumerator Count()
    {
        for(int i = 0; i < numbers.Count; i++)
        {
            if (i == numbers.Count -1)
            {
                releaseSound.Play();
            }
            else {
                countSound.Play();
            }

            imageRenderer.sprite = numbers[i];
            animator.SetTrigger("Animation");
            yield return new WaitForSecondsRealtime(1f);
        }
        transform.parent.gameObject.SetActive(false);
    }

    public static void StartCounting()
    {
        instance.transform.parent.gameObject.SetActive(true);
		Time.timeScale = 0f;
		VolumeController.SetProfile("Menu Volume Profile");
	}
}
